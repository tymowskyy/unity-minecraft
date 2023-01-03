using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int[,,] blocks;
    public ChunkRenderer terrain;

    // Start is called before the first frame update
    void Awake()
    {
        blocks = new int[Settings.CHUNK_WIDTH, Settings.CHUNK_HEIGHT, Settings.CHUNK_WIDTH];
    }

    public static bool IsInChunk(int x, int y, int z)
    {
        return x >= 0 && x < Settings.CHUNK_WIDTH
            && y >= 0 && y < Settings.CHUNK_HEIGHT
            && z >= 0 && z < Settings.CHUNK_WIDTH;
    }
}
