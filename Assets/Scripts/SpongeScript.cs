using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpongeScript : MonoBehaviour
{
    public int verticeNumber;
    public AnimationCurve topSqueezeCurve;
    public AnimationCurve topUnSqueezeCurve;
    public AnimationCurve bottomSqueezeCurve;
    public AnimationCurve bottomUnSqueezeCurve;
    public AnimationCurve squeezeCurve;
    [Range(0, 1)]
    public float squeezeValue;

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
        List<Vector2> uvs = new List<Vector2>();

        // Insert vertices at the top and bottom of the plane based on subdivisions.
        for (int i = 0; i < verticeNumber; i++)
        {
            // Calculate the percentage of the way through the plane we are
            float percentage = (float) i / (float)(verticeNumber - 1);

            float s = squeezeCurve.Evaluate(squeezeValue);
            float topSqueezeValue = Mathf.Lerp(topUnSqueezeCurve.Evaluate(percentage), topSqueezeCurve.Evaluate(percentage), s);
            float bottomSqueezeValue = Mathf.Lerp(bottomUnSqueezeCurve.Evaluate(percentage), bottomSqueezeCurve.Evaluate(percentage), s);
            
            // Insert vertex at the top and bottom of the plane
            vertices.Add(new Vector3(percentage, 1f - topSqueezeValue, 0f));
            uvs.Add(new Vector2(percentage, 1f));
            
            vertices.Add(new Vector3(percentage, 0 + bottomSqueezeValue, 0f));
            uvs.Add(new Vector2(percentage, 0));
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
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        // Insert mesh into mesh filter
        _meshFilter.mesh = mesh;
    }

    private void OnValidate()
    {
        GenerateMesh();
    }
    
    public void Squeeze(float value)
    {
        // Clamp value between 0 and 1
        squeezeValue = Mathf.Clamp01(value + squeezeValue);
        GenerateMesh();
    }
}
