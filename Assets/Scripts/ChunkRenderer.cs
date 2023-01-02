using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ChunkRenderer : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        UpdateChunk();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateChunk()
    {
        vertices = new Vector3[(Settings.CHUNK_SIZE + 1) * (Settings.CHUNK_SIZE + 1)];

        for (int x = 0; x <= Settings.CHUNK_SIZE; ++x)
        {
            for (int z = 0; z <= Settings.CHUNK_SIZE; ++z)
            {
                float y = Random.Range(0f, 1f);
                vertices[x * (Settings.CHUNK_SIZE + 1) + z] = new Vector3(x, y, z);
            }
        }

        triangles = new int[6 * Settings.CHUNK_SIZE * Settings.CHUNK_SIZE];

        for (int x=0; x < Settings.CHUNK_SIZE; ++x)
        {
            for (int z=0; z < Settings.CHUNK_SIZE; ++z)
            {
                int triange_index = 6 * (x * Settings.CHUNK_SIZE + z);

                triangles[triange_index + 0] = x * (Settings.CHUNK_SIZE + 1) + z;
                triangles[triange_index + 1] = x * (Settings.CHUNK_SIZE + 1) + z + 1;
                triangles[triange_index + 2] = (x+1) * (Settings.CHUNK_SIZE + 1) + z;

                triangles[triange_index + 3] = x * (Settings.CHUNK_SIZE + 1) + z + 1;
                triangles[triange_index + 4] = (x+1) * (Settings.CHUNK_SIZE + 1) + z + 1;
                triangles[triange_index + 5] = (x+1) * (Settings.CHUNK_SIZE + 1) + z;
                    
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
