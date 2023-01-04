using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TerrainGenerator))]
public class ChunkManager : MonoBehaviour
{
    public GameObject chunk_prefab;
    private Dictionary<Vector2, GameObject> chunks;
    private readonly int[,,] empty_chunk = new int[Settings.CHUNK_WIDTH, Settings.CHUNK_HEIGHT, Settings.CHUNK_WIDTH];
    private TerrainGenerator terrainGenerator;

    // Start is called before the first frame update
    void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();
        chunks = new Dictionary<Vector2, GameObject>();
        GenerateWorld();
        RenderWorld();
    }

    private void GenerateWorld()
    {
        for(int x=-Settings.RENDER_DISTANCE; x<Settings.RENDER_DISTANCE; ++x)
        {
            for(int z=-Settings.RENDER_DISTANCE; z<Settings.RENDER_DISTANCE; ++z)
            {
                GenerateChunk(x, z);
            }
        }
    }

    private void GenerateChunk(int chunk_x, int chunk_z)
    {
        GameObject chunk = Instantiate(
            chunk_prefab,
            new Vector3(Settings.CHUNK_WIDTH * chunk_x, 0, Settings.CHUNK_WIDTH * chunk_z),
            Quaternion.identity,
            transform
        );
        chunks.Add(
            new Vector2(chunk_x, chunk_z),
            chunk
        );

        chunk.GetComponent<Chunk>().blocks = terrainGenerator.GenerateChunk(chunk_x, chunk_z);

    }

    private void RenderWorld()
    {
        foreach (var item in chunks)
        {
            Chunk chunk_component = item.Value.GetComponent<Chunk>();

            chunk_component.terrain.UpdateChunk(
                chunk_component.blocks,
                GetChunkBlocks(item.Key + new Vector2(1, 0)),
                GetChunkBlocks(item.Key + new Vector2(-1, 0)),
                GetChunkBlocks(item.Key + new Vector2(0, 1)),
                GetChunkBlocks(item.Key + new Vector2(0, -1))
            );
        }
    }

    private int[,,] GetChunkBlocks(Vector2 chunk_pos)
    {
        return
            chunks.ContainsKey(chunk_pos) ?
            chunks[chunk_pos].GetComponent<Chunk>().blocks :
            empty_chunk;
    }
}
