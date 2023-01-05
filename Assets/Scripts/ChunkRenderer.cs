using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ChunkRenderer : MonoBehaviour
{
    private const float EPSILON = 0.00001f;
    private Mesh mesh;
    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uvs;
    private Block[,,] blocks, blocks_plus_x, blocks_minus_x, blocks_plus_z, blocks_minus_z;

    private Dictionary<FaceOrientation, Vector3[]> face_offsets = new Dictionary<FaceOrientation, Vector3[]>()
    {
        {
            FaceOrientation.Up, new Vector3[] {
                new Vector3(1, 1, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 1),
                new Vector3(1, 1, 1),
            }
        },
        {
            FaceOrientation.Down, new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 1),
                new Vector3(0, 0, 1),
            }
        },
        {
            FaceOrientation.Left, new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 0, 0),
            }
        },
        {
            FaceOrientation.Right, new Vector3[] {
                new Vector3(1, 0, 1),
                new Vector3(1, 1, 1),
                new Vector3(0, 1, 1),
                new Vector3(0, 0, 1),
            }
        },
        {
            FaceOrientation.Front, new Vector3[] {
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 1, 1),
                new Vector3(1, 0, 1),
            }
        },
        {
            FaceOrientation.Back, new Vector3[] {
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 1),
                new Vector3(0, 1, 0),
                new Vector3(0, 0, 0),
            }
        },
    };


    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void UpdateChunk(Block[,,] new_blocks, Block[,,] new_blocks_plus_x, Block[,,] new_blocks_minus_x, Block[,,] new_blocks_plus_z, Block[,,] new_blocks_minus_z)
    {
        blocks = new_blocks;
        blocks_plus_x = new_blocks_plus_x;
        blocks_minus_x = new_blocks_minus_x;
        blocks_plus_z = new_blocks_plus_z;
        blocks_minus_z = new_blocks_minus_z;
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        for (int x = 0; x < Settings.CHUNK_WIDTH; ++x)
        {
            for (int y = 0; y < Settings.CHUNK_HEIGHT; ++y)
            {
                for (int z = 0; z < Settings.CHUNK_WIDTH; ++z)
                {
                    UpdateBlock(x, y, z);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    private void UpdateBlock(int x, int y, int z)
    {
        if (!Chunk.IsInChunk(x, y, z) ||
            blocks[x, y, z] == Block.Air) return;

        Vector3 vertex = new Vector3(x, y, z);
        if (GetBlock(x - 1, y, z) == Block.Air)
            UpdateFace(vertex, blocks[x, y, z], FaceOrientation.Back);

        if (GetBlock(x + 1, y, z) == Block.Air)
            UpdateFace(vertex, blocks[x, y, z], FaceOrientation.Front);

        if (GetBlock(x, y - 1, z) == Block.Air)
            UpdateFace(vertex, blocks[x, y, z], FaceOrientation.Down);

        if (GetBlock(x, y + 1, z) == Block.Air)
            UpdateFace(vertex, blocks[x, y, z], FaceOrientation.Up);

        if (GetBlock(x, y, z - 1) == Block.Air)
            UpdateFace(vertex, blocks[x, y, z], FaceOrientation.Left);

        if (GetBlock(x, y, z + 1) == Block.Air)
            UpdateFace(vertex, blocks[x, y, z], FaceOrientation.Right);
    }

    private Block GetBlock(int x, int y, int z)
    {
        if (y < 0 || y >= Settings.CHUNK_HEIGHT) return Block.Air;
        if (Chunk.IsInChunk(x, y, z)) return blocks[x, y, z];
        if (x < 0) return blocks_minus_x[Settings.CHUNK_WIDTH-1, y, z];
        if (x >= Settings.CHUNK_WIDTH) return blocks_plus_x[0, y, z];
        if (z < 0) return blocks_minus_z[x, y, Settings.CHUNK_WIDTH-1];
        return blocks_plus_z[x, y, 0];
    }

    private void UpdateFace(Vector3 vertex, Block block, FaceOrientation faceOrientation)
    {
        int index = vertices.Count;

        foreach (Vector3 vertex_offset in face_offsets[faceOrientation])
            vertices.Add(vertex + vertex_offset);

        triangles.AddRange(new int[] {
            index + 0,
            index + 1,
            index + 2,
            index + 2,
            index + 3,
            index + 0,
        });

        uvs.AddRange(
            BlockData.GetUVCoordinates(
                BlockData.spritePositions[block][faceOrientation]
        ));
    }
}
 