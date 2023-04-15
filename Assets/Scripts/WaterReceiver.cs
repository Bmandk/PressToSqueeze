using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterReceiver : MonoBehaviour
{
    // A script that cycles through different sprites based on the amount of water dropped on it.
    // The sprites should be in order from least water to most water.
    
    public GameObject[] sprites;
    public int dropsPerSprite = 5;
    public int currentDrops = 0;
    private int index = 0;

    public void AddWater(int amount)
    {
        // Change the sprite when the amount of water reaches a certain threshold.
        currentDrops += amount;
        if (currentDrops >= dropsPerSprite)
        {
            currentDrops -= dropsPerSprite;
            sprites[index].SetActive(false);
            index++;
            if (index >= sprites.Length)
            {
                index = sprites.Length - 1;
            }
            sprites[index].SetActive(true);
        }
    }
}
