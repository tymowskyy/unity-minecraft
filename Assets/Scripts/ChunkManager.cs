using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunk_prefab;
    private Dictionary<Vector2, GameObject> chunks;

    // Start is called before the first frame update
    void Start()
    {
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
        for (int x = 0; x < Settings.CHUNK_WIDTH; ++x)
        {
            for (int z = 0; z < Settings.CHUNK_WIDTH; ++z)
            {
                int height = Random.Range(16, 20);
                for (int y = 0; y < height; ++y)
                {
                    chunk.GetComponent<Chunk>().blocks[x, y, z] = 1;
                }
            }
        }
    }

    private void RenderWorld()
    {
        foreach (var item in chunks)
        {
            Chunk chunk_component = item.Value.GetComponent<Chunk>();
            chunk_component.terrain.UpdateChunk(ref chunk_component.blocks);
        }
    }
}
