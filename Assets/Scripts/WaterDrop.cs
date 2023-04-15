using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("WaterDrop"))
            return;

        var waterReceiver = other.GetComponent<WaterReceiver>();
        if (waterReceiver != null) 
            waterReceiver.AddWater(1);

        Destroy(gameObject);
    }
}
