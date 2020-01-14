using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class mesh_generator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;



    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        create_shape();
        update_mesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void create_shape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.5f, z * 0.5f) *2f - Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 10f + Mathf.PerlinNoise(x * 0.001f, z * 0.001f) * 100f - 50;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }



        int vert = 0;
        int tris = 0;
        triangles = new int[xSize * zSize * 6];

        for (int z = 0; z < zSize; z++)
        {

            for (int x = 0; x < xSize; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }



    }

    void update_mesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    /*private void OnDrawGizmos()
    {
        if(vertices == null)
        {
            return;
        }
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }*/
}
