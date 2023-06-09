using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    
    [Range(0, 100f)]
    public float waterAbsorbRate = 0.01f;
    [Range(0, 100f)]
    public float waterSqueezeRate = 0.01f;
    public float currentWaterAmount;
    public Gradient waterColorGradient;
    public GameObject waterDropPrefab;
    public float waterPerDrop = 0.1f;
    public float currentWaterDropAmount;
    public float waterDropSpawnRadius;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Vector3 originalScale;

    public bool changingSize = false;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        originalScale = transform.localScale;
    }

    [ContextMenu("Generate")]
    public void GenerateMesh()
    {
        // Get mesh filter if null
        if (_meshFilter == null)
        {
            _meshFilter = GetComponentInChildren<MeshFilter>();
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
            vertices.Add(new Vector3(percentage - 0.5f, 0.5f - topSqueezeValue, 0f));
            uvs.Add(new Vector2(percentage, 1f));
            
            vertices.Add(new Vector3(percentage - 0.5f, -0.5f + bottomSqueezeValue, 0f));
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
        if (value > 0)
            AddWaterAmount(-waterSqueezeRate * value);
        else {
            changingSize = false;
        }
        GenerateMesh();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            AddWaterAmount(waterAbsorbRate * Time.fixedDeltaTime);
        }
    }
    
    public AnimationCurve waterScaleCurve;
    public float waterDropForce;
    public float waterDropAngle;
    
    public void AddWaterAmount(float amount)
    {
        currentWaterAmount = Mathf.Clamp01(currentWaterAmount + amount);
        var scale = waterScaleCurve.Evaluate(currentWaterAmount);
        transform.localScale = originalScale * scale;
        var waterColor = waterColorGradient.Evaluate(currentWaterAmount);
        _meshRenderer.material.color = waterColor;
        changingSize = true;
        if (currentWaterAmount > -amount)
        {
            currentWaterDropAmount += Mathf.Max(-amount, 0);
            
            while (currentWaterDropAmount > waterPerDrop)
            {
                currentWaterDropAmount -= waterPerDrop;
                var position = transform.position + (Vector3)Random.insideUnitCircle * waterDropSpawnRadius;
                var waterDrop = Instantiate(waterDropPrefab, position, Quaternion.identity);
                float angle = Random.Range(-waterDropAngle, waterDropAngle) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                waterDrop.GetComponent<Rigidbody2D>().velocity = dir * waterDropForce;
            }
        }
    }
}
