using System;
using UnityEngine;

public class WaterShadow : MonoBehaviour
{
    public float fadeTime = 1f;
    public float fadeSpeed = 1f;
    private SpriteRenderer _spriteRenderer;
    
    private void Awake()
    {
        Destroy(gameObject, fadeTime);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var color = _spriteRenderer.color;
        color.a -= fadeSpeed * Time.deltaTime;
        _spriteRenderer.color = color;
    }
}