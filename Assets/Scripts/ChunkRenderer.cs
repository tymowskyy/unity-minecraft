using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ChunkRenderer : MonoBehaviour
{
    private Mesh mesh;
    private List<Vector3> vertices;
    private List<int> triangles;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void UpdateChunk(ref int[,,] blocks)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < Settings.CHUNK_WIDTH; ++x)
        {
            for (int y = 0; y < Settings.CHUNK_HEIGHT; ++y)
            {
                for (int z = 0; z < Settings.CHUNK_WIDTH; ++z)
                {
                    UpdateBlock(x, y, z, ref blocks);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    private void UpdateBlock(int x, int y, int z, ref int[,,] blocks)
    {
        if (!Chunk.IsInChunk(x, y, z) ||
            blocks[x, y, z] == 0) return;

        Vector3 vertex = new Vector3(x, y, z);
        if (Chunk.IsInChunk(x - 1, y, z) && blocks[x - 1, y, z] == 0)
            UpdateFace(vertex, new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 1),
            });
        if (Chunk.IsInChunk(x + 1, y, z) && blocks[x + 1, y, z] == 0)
            UpdateFace(vertex, new Vector3[] {
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 0, 1),
                new Vector3(1, 1, 1),
            });

        if (Chunk.IsInChunk(x, y-1, z) && blocks[x, y-1, z] == 0)
            UpdateFace(vertex, new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
            });

        if (Chunk.IsInChunk(x, y + 1, z) && blocks[x, y + 1, z] == 0)
            UpdateFace(vertex, new Vector3[] {
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 1),
                new Vector3(1, 1, 0),
                new Vector3(1, 1, 1),
            });

        if (Chunk.IsInChunk(x, y, z - 1) && blocks[x, y, z - 1] == 0)
            UpdateFace(vertex, new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 0),
            });

        if (Chunk.IsInChunk(x, y, z + 1) && blocks[x, y, z + 1] == 0)
            UpdateFace(vertex, new Vector3[] {
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
                new Vector3(0, 1, 1),
                new Vector3(1, 1, 1),
            });
    }

    private void UpdateFace(Vector3 vertex, Vector3[] vertices_offsets)
    {
        int index = vertices.Count;

        foreach (Vector3 vertex_offset in vertices_offsets)
            vertices.Add(vertex + vertex_offset);

        triangles.AddRange(new int[] {
            index + 0,
            index + 1,
            index + 2,
            index + 2,
            index + 1,
            index + 3,
        });
    }
}
 