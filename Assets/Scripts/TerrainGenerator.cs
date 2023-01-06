using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public AnimationCurve heightCurve;

    public float HEIGHT_NOISE1_SCALE = 1f;
    public float HEIGHT_NOISE2_SCALE = 2f;
    public float HEIGHT_NOISE_WEIGHT = 0.6f;

    public float forestNoiseScale = 1f;
    public float minForestLevel = 0.5f;
    public int minTrees = 2;
    public int maxTrees = 6;
    public int minTreeHeight = 3;
    public int maxTreeHeight = 6;

    private FastNoiseLite heightNoise1, heightNoise2, dirtLevelNoise, forestNoise;
    private int seed;
    private int[,] height = new int[Settings.CHUNK_WIDTH, Settings.CHUNK_WIDTH];
    private int[,] dirtLevel = new int[Settings.CHUNK_WIDTH, Settings.CHUNK_WIDTH];
    private float forestLevel;

    public const int BASE_DIRT_LEVEL = 5;
    public const float DIRT_LEVEL_MULTIPLICATOR = 4f;

    private void Awake()
    {
        seed = Random.Range(0, 10000);
        Debug.Log("Seed: " + seed);
        System.Random rand = new System.Random(seed);

        heightNoise1 = new FastNoiseLite(rand.Next() % 100000);
        heightNoise2 = new FastNoiseLite(rand.Next() % 100000);
        dirtLevelNoise = new FastNoiseLite(rand.Next() % 100000);
        forestNoise = new FastNoiseLite(rand.Next() % 100000);
        heightNoise1.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        heightNoise2.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        dirtLevelNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        forestNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
    }

    public Block[,,] GenerateChunk(int chunkX, int chunkZ)
    {
        Block[,,] blocks = new Block[Settings.CHUNK_WIDTH, Settings.CHUNK_HEIGHT, Settings.CHUNK_WIDTH];
        for (int x = 0; x < Settings.CHUNK_WIDTH; ++x)
        {
            for (int z = 0; z < Settings.CHUNK_WIDTH; ++z)
            {

                float heightNoiseValue = MapToZeroOne(Lerp(
                    heightNoise1.GetNoise(
                        HEIGHT_NOISE1_SCALE * (float)(chunkX * Settings.CHUNK_WIDTH + x),
                        HEIGHT_NOISE1_SCALE * (float)(chunkZ * Settings.CHUNK_WIDTH + z)
                    ),
                    heightNoise2.GetNoise(
                        HEIGHT_NOISE2_SCALE * (float)(chunkX * Settings.CHUNK_WIDTH + x),
                        HEIGHT_NOISE2_SCALE * (float)(chunkZ * Settings.CHUNK_WIDTH + z)
                    ),
                    HEIGHT_NOISE_WEIGHT
                ));
                height[x, z] = (int)heightCurve.Evaluate(heightNoiseValue); 
                dirtLevel[x, z] = BASE_DIRT_LEVEL + (int)(DIRT_LEVEL_MULTIPLICATOR * dirtLevelNoise.GetNoise(chunkX * Settings.CHUNK_WIDTH + x, chunkZ * Settings.CHUNK_WIDTH + z));
            }
        }

        forestLevel = MapToZeroOne(forestNoise.GetNoise(
            forestNoiseScale * (float) chunkX * Settings.CHUNK_WIDTH,
            forestNoiseScale * (float) chunkZ * Settings.CHUNK_WIDTH
        ));

        for (int x = 0; x < Settings.CHUNK_WIDTH; ++x)
        {
            for (int y = 0; y < Settings.CHUNK_HEIGHT; ++y)
            {
                for (int z = 0; z < Settings.CHUNK_WIDTH; ++z)
                {
                    blocks[x, y, z] = GenerateBlock(x, y, z);
                }
            }
        }


        if(forestLevel >= minForestLevel)
        {
            System.Random randTrees = new System.Random(chunkX * 100000 + chunkZ);
            int nTrees = randTrees.Next(minTrees, maxTrees);

            for(int i=0; i<nTrees; ++i)
            {
                int x, y;
                x = randTrees.Next(0, Settings.CHUNK_WIDTH - 1);
                y = randTrees.Next(0, Settings.CHUNK_WIDTH - 1);

                MakeTree(x, y, blocks, randTrees.Next(minTreeHeight, maxTreeHeight));
            }
        }

        return blocks;
    }

    private Block GenerateBlock(int x, int y, int z)
    {
        if (y > height[x, z]) return Block.Air;
        else if (y == height[x, z]) return Block.Grass;
        else if (y > height[x, z] - dirtLevel[x, z]) return Block.Dirt;
        return Block.Stone;
    }

    private float MapToZeroOne(float x)
    {
        return (x + 1f) / 2;
    }

    private float Lerp(float a, float b, float t)
    {
        return a * t + b * (1 - t);
    }

    private void MakeTree(int x, int z, Block[,,] blocks, int treeHeight)
    {
        int h = height[x, z];

        for(int i=0; i<treeHeight; ++i)
            PlaceBlockInChunk(x, h + i + 1, z, Block.Log, blocks);

        PlaceBlockInChunk(x + 1, h + treeHeight, z, Block.Leaves, blocks);
        PlaceBlockInChunk(x - 1, h + treeHeight, z, Block.Leaves, blocks);
        PlaceBlockInChunk(x, h + treeHeight, z + 1, Block.Leaves, blocks);
        PlaceBlockInChunk(x, h + treeHeight, z - 1, Block.Leaves, blocks);
        PlaceBlockInChunk(x, h + treeHeight + 1, z, Block.Leaves, blocks);
    }
    
    private void PlaceBlockInChunk(int x, int y, int z, Block block, Block[,,] blocks)
    {
        if (Chunk.IsInChunk(x, y, z)) blocks[x, y, z] = block;
    }

}
