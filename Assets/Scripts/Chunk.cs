using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private int[,,] blocks;
    public ChunkRenderer terrain;

    // Start is called before the first frame update
    void Start()
    {
        GenerateChunk();
        terrain.OnStart();
        terrain.UpdateChunk(ref blocks);
    }

    public void GenerateChunk()
    {
        blocks = new int[Settings.CHUNK_WIDTH, Settings.CHUNK_HEIGHT, Settings.CHUNK_WIDTH];

        for (int x=0; x<Settings.CHUNK_WIDTH; ++x)
        {
            for (int z=0; z<Settings.CHUNK_WIDTH; ++z)
            {
                int height = Random.Range(16, 20);
                for(int y=0; y<height; ++y)
                {
                    blocks[x, y, z] = 1;
                }
            }
        }

    }
    public static bool IsInChunk(int x, int y, int z)
    {
        return x >= 0 && x < Settings.CHUNK_WIDTH
            && y >= 0 && y < Settings.CHUNK_HEIGHT
            && z >= 0 && z < Settings.CHUNK_WIDTH;
    }
}
