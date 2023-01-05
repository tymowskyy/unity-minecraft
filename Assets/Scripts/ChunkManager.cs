using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TerrainGenerator))]
public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    private Dictionary<Vector2, GameObject> chunks;
    private readonly Block[,,] emptyChunk = new Block[Settings.CHUNK_WIDTH, Settings.CHUNK_HEIGHT, Settings.CHUNK_WIDTH];
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

    private void GenerateChunk(int chunkX, int chunkZ)
    {
        GameObject chunk = Instantiate(
            chunkPrefab,
            new Vector3(Settings.CHUNK_WIDTH * chunkX, 0, Settings.CHUNK_WIDTH * chunkZ),
            Quaternion.identity,
            transform
        );
        chunks.Add(
            new Vector2(chunkX, chunkZ),
            chunk
        );

        chunk.GetComponent<Chunk>().blocks = terrainGenerator.GenerateChunk(chunkX, chunkZ);

    }

    private void RenderWorld()
    {
        foreach (var item in chunks)
        {
            Chunk chunkComponent = item.Value.GetComponent<Chunk>();

            chunkComponent.terrain.UpdateChunk(
                chunkComponent.blocks,
                GetChunkBlocks(item.Key + new Vector2(1, 0)),
                GetChunkBlocks(item.Key + new Vector2(-1, 0)),
                GetChunkBlocks(item.Key + new Vector2(0, 1)),
                GetChunkBlocks(item.Key + new Vector2(0, -1))
            );
        }
    }

    private Block[,,] GetChunkBlocks(Vector2 chunkPos)
    {
        return
            chunks.ContainsKey(chunkPos) ?
            chunks[chunkPos].GetComponent<Chunk>().blocks :
            emptyChunk;
    }
}
