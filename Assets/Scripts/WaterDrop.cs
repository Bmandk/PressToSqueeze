using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{


    public List<AudioClip> waterDropletAudioClips;
    public GameObject waterShadowPrefab;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

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


        Instantiate(waterShadowPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Rotate rigidbody with rotation of velocity.
        var velocity = _rigidbody2D.velocity;
        if (velocity.magnitude > 0.1f)
        {
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90;
            _rigidbody2D.rotation = angle;
        }
    }
}
