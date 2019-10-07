using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSDistort : MonoBehaviour
{
    public float minTimeToDistort, maxTimeToDistort;
    public Material screenDistort;

    private void Start()
    {
        StartCoroutine(DistortImg());
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, screenDistort);
    }

    IEnumerator DistortImg()
    {
        float timer = 0;
        while (true)
        {
            timer = Random.Range(minTimeToDistort, maxTimeToDistort);
            yield return new WaitForSecondsRealtime(timer);
            screenDistort.SetFloat("_Magnitude", Random.Range(.01f, .09f));
            yield return new WaitForSecondsRealtime(.05f);
            screenDistort.SetFloat("_Magnitude", 0);
        }
    }
}
