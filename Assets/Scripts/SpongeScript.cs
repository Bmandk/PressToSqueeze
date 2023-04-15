using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpongeScript : MonoBehaviour
{
    public int verticeNumber;

    private MeshFilter _meshFilter;

    [ContextMenu("Generate")]
    public void GenerateMesh()
    {
        // Get mesh filter if null
        if (_meshFilter == null)
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
        
        // Create mesh
        Mesh mesh = new Mesh();
        
        // Populate mesh with plane vertices
        List<Vector3> vertices = new List<Vector3>();
        
        // Insert vertices at the top and bottom of the plane based on subdivisions.
        for (int i = 0; i < verticeNumber; i++)
        {
            // Calculate the percentage of the way through the plane we are
            float percentage = (float) i / (float) verticeNumber;
            
            // Insert vertex at the top and bottom of the plane
            vertices.Add(new Vector3(percentage, 0.5f, 0f));
            vertices.Add(new Vector3(percentage, -0.5f, 0f));
        }
        
        List<int> triangles = new List<int>();

        for (int i = 0; i < verticeNumber - 1; i++)
        {
            // Left triangle
            triangles.Add(i * 2);
            triangles.Add(i * 2 + 2);
            triangles.Add(i * 2 + 1);
            // Right triangle
            triangles.Add(i * 2 + 1);
            triangles.Add(i * 2 + 2);
            triangles.Add(i * 2 + 3);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        // Insert mesh into mesh filter
        _meshFilter.mesh = mesh;
    }
}
