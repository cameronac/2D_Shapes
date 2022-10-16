using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using UnityEngine;
using UnityEngine.UI;


public class UIShapes : MonoBehaviour
{
    Vector3[] vertices;
    Mesh mesh;

    private void Start() {
        
        //Create Mesh
        mesh = new Mesh();
        mesh.name = "Circle";
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        
        //Mesh Data
        LineCircle lineCircle = new LineCircle(30, 20, 2f);
        
        //Assigning Results to Mesh 
        mesh.vertices = lineCircle.vertices;
        mesh.uv = lineCircle.uv;
        mesh.triangles = lineCircle.triangles;
        meshFilter.mesh = mesh; //Passing out our Mesh reference
    }

    void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            vertices[0] = new Vector3(0, 100);
            mesh.vertices = vertices;
            Debug.Log("Updated Vertices");
        }
    }
}