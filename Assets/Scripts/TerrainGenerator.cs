using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private FastNoiseLite noise;
    private int seed;
    private int[,] height_noise = new int[Settings.CHUNK_WIDTH, Settings.CHUNK_WIDTH];

    private const int BASE_HEIGHT = 32;
    private const float HEIGHT_MULTIPLICATOR = 10f;

    private void Awake()
    {
        seed = Random.Range(0, 10000);
        noise = new FastNoiseLite(seed);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        Debug.Log("Seed: " + seed);
    }

    public Block[,,] GenerateChunk(int chunk_x, int chunk_z)
    {
        Block[,,] blocks = new Block[Settings.CHUNK_WIDTH, Settings.CHUNK_HEIGHT, Settings.CHUNK_WIDTH];
        for (int x = 0; x < Settings.CHUNK_WIDTH; ++x)
        {
            for (int z = 0; z < Settings.CHUNK_WIDTH; ++z)
            {
                height_noise[x, z] = BASE_HEIGHT + (int)(HEIGHT_MULTIPLICATOR * noise.GetNoise(chunk_x * Settings.CHUNK_WIDTH + x, chunk_z * Settings.CHUNK_WIDTH + z));
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
        if (y > height_noise[x, z]) return Block.Air;
        else if (y == height_noise[x, z]) return Block.Grass;
        return Block.Dirt;
    }
}
