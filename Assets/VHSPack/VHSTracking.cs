using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSTracking : MonoBehaviour
{
    public float minTimeToTrack, maxTimeToTrack;
    public Material screenTracking;

    private void Start()
    {
        StartCoroutine(StartTrack());
    }


    private void Update()
    {
        screenTracking.SetFloat("_TimeValue", Time.unscaledTime);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, screenTracking);
    }

    IEnumerator StartTrack()
    {
        float timer = 0;
        while (true)
        {
            timer = Random.Range(minTimeToTrack, maxTimeToTrack);
            yield return new WaitForSecondsRealtime(timer);
            screenTracking.SetFloat("_Scan", 1);
            screenTracking.SetFloat("_Speed", Random.Range(.1f, .4f));
            screenTracking.SetFloat("_Line Width", Random.Range(.95f, .988f));
            timer = Random.Range(4f, 6f);
            yield return new WaitForSecondsRealtime(timer);
            screenTracking.SetFloat("_Scan", 0);
        }
    }
}
