using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomReveal : MonoBehaviour
{
    public Tilemap room;
    public Color tilemapColor;
    public bool hidden = true;

    private void Start()
    {
        room = GetComponent<Tilemap>();
        room.color = tilemapColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerVision")
        {
            StartCoroutine(FadeTilesOut());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerVision")
        {
            StartCoroutine(FadeTilesIn());
        }
    }

    //private void OnTriggerExit(Collider other)
    //{       
    //    if(other.transform.tag == "Player")
    //    {
    //        hidden = !hidden;
    //        if (hidden)
    //        {
    //            StartCoroutine(FadeTilesIn());
    //        }
    //        else
    //        {
    //            StartCoroutine(FadeTilesOut());
    //        }
    //    }
    //}

    IEnumerator FadeTilesIn()
    {
        //float startSpeed = .5f;
        float lerpTime = .5f;
        float currentLerpTime = 0;
        Color currentColor = room.color;

        while (currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;
            room.color = Color.Lerp(currentColor, new Color(currentColor.r, currentColor.g, currentColor.b, 1f), perc);

            yield return null;
        }
    }

    IEnumerator FadeTilesOut()
    {
        //float startSpeed = .5f;
        float lerpTime = .5f;
        float currentLerpTime = 0;
        Color currentColor = room.color;

        while (currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;
            room.color = Color.Lerp(currentColor, new Color(currentColor.r, currentColor.g, currentColor.b, 0f), perc);

            yield return null;
        }
    }
}
