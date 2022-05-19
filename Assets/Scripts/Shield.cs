using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Hexasphere;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Shield : MonoBehaviour
{
    public float radius = 2f;
    public int divisions = 7;
    public float hexSize = .9f;

    private MeshFilter meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = GenerateShieldMesh();
    }

    Mesh GenerateShieldMesh()
    {
        Hexasphere hexasphere = new Hexasphere(radius, divisions, hexSize);

        Mesh mesh = new();
        mesh.vertices = hexasphere.MeshDetails.Vertices.ToArray();
        mesh.triangles = hexasphere.MeshDetails.Triangles.ToArray();
        mesh.RecalculateNormals();
        
        return mesh;
    }
}
