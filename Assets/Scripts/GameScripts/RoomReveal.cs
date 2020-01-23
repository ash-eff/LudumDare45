using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomReveal : MonoBehaviour
{
    public Tilemap room;
    public Color tilemapColor;
    public bool roomHidden = true;

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

    IEnumerator FadeTilesIn()
    {
        roomHidden = true;
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
        roomHidden = false;
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
