using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSVDistort : MonoBehaviour
{
    public float minTimeToScan, maxTimeToScan;
    public Material screenVDistort;
    float currentThickness = 0;

    private void Start()
    {
        StartCoroutine(StartScans());
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, screenVDistort);
    }

    private void Update()
    {
        screenVDistort.SetFloat("_TimeValue", Time.unscaledTime);
    }

    IEnumerator StartScans()
    {
        float timer = 0;
        while (true)
        {
            timer = Random.Range(minTimeToScan, maxTimeToScan);
            yield return new WaitForSecondsRealtime(timer);
            screenVDistort.SetFloat("_Scan", 1);
            screenVDistort.SetFloat("_Speed", Random.Range(.1f, .3f));
            screenVDistort.SetFloat("_LineThickness", Random.Range(.03f, .12f));
            currentThickness = screenVDistort.GetFloat("_LineThickness");
            timer = Random.Range(3, 5);
            yield return new WaitForSecondsRealtime(timer);
            StartCoroutine(ShrinkSize());
        }
    }

    IEnumerator ShrinkSize()
    {
        while(currentThickness > 0f)
        {
            currentThickness -= Time.deltaTime * 10f;
            screenVDistort.SetFloat("_LineThickness", currentThickness);
            yield return null;
        }

        screenVDistort.SetFloat("_Scan", 0);
    }
}
