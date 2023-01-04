using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Block[,,] blocks;
    public ChunkRenderer terrain;

    public static bool IsInChunk(int x, int y, int z)
    {
        return x >= 0 && x < Settings.CHUNK_WIDTH
            && y >= 0 && y < Settings.CHUNK_HEIGHT
            && z >= 0 && z < Settings.CHUNK_WIDTH;
    }
}
