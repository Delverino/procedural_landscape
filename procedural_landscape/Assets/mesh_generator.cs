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

    private float randMult;
    private int type2;
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
        minHeight = float.MaxValue;
        maxHeight = float.MinValue;
        randMult = Random.Range(0.6f, 2f);
        type = Random.Range(0, 6);
        seed = Random.Range(0, 100);
        type2 = Random.Range(0, 4);
        float randomX = Random.Range(0, (float)xSize);
        float randomZ = Random.Range(0, (float)zSize);

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = -myNoise(x * 0.5f, z * 0.5f) +myNoise(x * 0.1f, z * 0.1f) * myNoise(x * 0.1f, z * 0.1f) * 10; //+ Mathf.PerlinNoise(x * 0.001f, z * 0.001f) * 100f - 50;
                if (type == 0)
                {
                    switch (type2)
                    {
                        case 0:
                            y = y * x / xSize;
                            break;

                        case 1:
                            y = y * (1 - ((float)x / xSize));
                            break;
                        case 2:
                            y = y * ((float)z / zSize);
                            break;
                        case 3:
                            y = y * (1 - ((float)z / zSize));
                            break;

                    }
                }
                else if (type == 1)
                {
                    y = y * Mathf.Sin((float)x * 2/xSize + z * 2/zSize);
                } else if(type == 2)
                {
                    y = y * 0.5f;
                } else if (type == 3)
                {
                    y = y * randMult;
                } else if(type == 4)
                {
                    float dist = Mathf.Pow(x - randomX, 2) + Mathf.Pow(z - randomZ, 2);
                    y = y + 4 - ( (dist / xSize))/4f;
                } else {
                    float dist = Mathf.Sqrt( Mathf.Pow(x - randomX, 2) + Mathf.Pow(z - randomZ, 2));
                    y = y * (2f * (dist / xSize));
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
