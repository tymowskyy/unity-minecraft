using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private FastNoiseLite heightNoise, dirtLevelNoise;
    private int seed;
    private int[,] height = new int[Settings.CHUNK_WIDTH, Settings.CHUNK_WIDTH];
    private int[,] dirtLevel = new int[Settings.CHUNK_WIDTH, Settings.CHUNK_WIDTH];

    private const int BASE_HEIGHT = 32;
    private const float HEIGHT_MULTIPLICATOR = 10f;

    private const int BASE_DIRT_LEVEL = 5;
    private const float DIRT_LEVEL_MULTIPLICATOR = 4f;

    private void Awake()
    {
        seed = Random.Range(0, 10000);
        Debug.Log("Seed: " + seed);
        System.Random rand = new System.Random(seed);

        heightNoise = new FastNoiseLite(rand.Next() % 100000);
        dirtLevelNoise = new FastNoiseLite(rand.Next() % 100000);
        heightNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        dirtLevelNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
    }

    public Block[,,] GenerateChunk(int chunk_x, int chunk_z)
    {
        Block[,,] blocks = new Block[Settings.CHUNK_WIDTH, Settings.CHUNK_HEIGHT, Settings.CHUNK_WIDTH];
        for (int x = 0; x < Settings.CHUNK_WIDTH; ++x)
        {
            for (int z = 0; z < Settings.CHUNK_WIDTH; ++z)
            {
                height[x, z] = BASE_HEIGHT + (int)(HEIGHT_MULTIPLICATOR * heightNoise.GetNoise(chunk_x * Settings.CHUNK_WIDTH + x, chunk_z * Settings.CHUNK_WIDTH + z));
                dirtLevel[x, z] = BASE_DIRT_LEVEL + (int)(DIRT_LEVEL_MULTIPLICATOR * dirtLevelNoise.GetNoise(chunk_x * Settings.CHUNK_WIDTH + x, chunk_z * Settings.CHUNK_WIDTH + z));
            }
        }

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

        return blocks;
    }

    private Block GenerateBlock(int x, int y, int z)
    {
        if (y > height[x, z]) return Block.Air;
        else if (y == height[x, z]) return Block.Grass;
        else if(y > height[x, z] - dirtLevel[x, z]) return Block.Dirt;
        return Block.Stone;
    }
}
