using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    public GameObject waterShadowPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("WaterDrop") || other.CompareTag("Water"))
            return;

        var waterReceiver = other.GetComponent<WaterReceiver>();
        if (waterReceiver != null) 
            waterReceiver.AddWater(1);

        Instantiate(waterShadowPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
