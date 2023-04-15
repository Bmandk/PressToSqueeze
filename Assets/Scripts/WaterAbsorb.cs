using System;
using UnityEngine;

public class WaterAbsorb : MonoBehaviour
{
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}