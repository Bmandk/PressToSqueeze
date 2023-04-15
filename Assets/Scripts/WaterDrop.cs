using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{

    public List<AudioClip> waterDropletAudioClips;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("WaterDrop") || other.CompareTag("Water"))
            return;

        var waterReceiver = other.GetComponent<WaterReceiver>();
        if (waterReceiver != null) 
            waterReceiver.AddWater(1);

        if (waterDropletAudioClips.Count > 0)
        {
            AudioSource.PlayClipAtPoint(waterDropletAudioClips[UnityEngine.Random.Range(0, waterDropletAudioClips.Count)], transform.position);
        }

        Destroy(gameObject);
    }
}
