using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WaterReceiver : MonoBehaviour
{
    // A script that cycles through different sprites based on the amount of water dropped on it.
    // The sprites should be in order from least water to most water.
    
    public GameObject[] sprites;
    public int dropsPerSprite = 5;
    public int currentDrops = 0;
    public Image image;
    public AnimationCurve fadeImageCurve;
    private int index = -1;
    public AudioClip upgradeClip;

    public void AddWater(int amount)
    {
        // Change the sprite when the amount of water reaches a certain threshold.
        currentDrops += amount;
        if (currentDrops >= dropsPerSprite)
        {
            currentDrops -= dropsPerSprite;
            if (index >= 0)
                sprites[index].SetActive(false);
            index++;
            if (index >= sprites.Length - 1)
            {
                index = sprites.Length - 1;
                WinGame();
            }
            sprites[index].SetActive(true);
            AudioSource.PlayClipAtPoint(upgradeClip, transform.position);
        }
    }

    private void WinGame()
    {
        StartCoroutine(FadeImage());
    }

    private IEnumerator FadeImage()
    {
        image.gameObject.SetActive(true);
        float startTime = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - startTime < fadeImageCurve.keys[fadeImageCurve.length - 1].time)
        {
            float percentage = (Time.timeSinceLevelLoad - startTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeImageCurve.Evaluate(percentage));
            yield return null;
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
