using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class mesh_generator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;


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
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0)
        };

        triangles = new int[]
        {
            0,1,2
        };

    }

    void update_mesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
