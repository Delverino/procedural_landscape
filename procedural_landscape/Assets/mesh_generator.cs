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

    public Gradient grad;

    Color[] colors;

    private float minHeight = float.MaxValue, maxHeight = float.MinValue;
    private int seed;
    private int type;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        create_shape();
        update_mesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            create_shape();
            update_mesh();
        }
    }

    public float myNoise(float x, float y)
    {
        return Mathf.PerlinNoise(x + seed, y + seed);
    }

    void create_shape()
    {
        type = Random.Range(0, 3);
        seed = Random.Range(0, 100);
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = -myNoise(x * 0.5f, z * 0.5f) +myNoise(x * 0.1f, z * 0.1f) * myNoise(x * 0.1f, z * 0.1f) * 10; //+ Mathf.PerlinNoise(x * 0.001f, z * 0.001f) * 100f - 50;
                if (type == 0)
                {
                    y = y * x / xSize;
                }
                else if (type == 1)
                {
                    y = y * Mathf.Sin((float)x * 2/xSize + z * 2/zSize);
                } else if(type == 2)
                {
                    y = y * 0.5f;
                }
                if (y > maxHeight)
                {
                    maxHeight = y;
                }
                if(y < minHeight)
                {
                    minHeight = y;
                }
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


        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x < xSize; x++, i++)
            {
                colors[i] = grad.Evaluate(Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y));
                

            }
        }


    }

    void update_mesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.colors = colors;
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
