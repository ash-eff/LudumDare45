using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarUI : WindowUI
{
    public Image progressBar;
    public TextMeshProUGUI progressText;
    public string startText, finishText;
    public float uploadTime;

    void Awake()
    {
        progressBar.fillAmount = 0;
        progressText.text = startText;
        StartCoroutine(IEFillBar());
    }

    IEnumerator IEFillBar()
    {
        float currentTime = 0;
        while (currentTime < uploadTime)
        {
            currentTime += Time.deltaTime;
            progressBar.fillAmount = currentTime / uploadTime;

            yield return null;
        }

        progressText.text = finishText;
        yield return new WaitForSeconds(.5f);
        this.gameObject.SetActive(false);
    }
}
