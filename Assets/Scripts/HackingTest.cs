using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HackingTest : MonoBehaviour
{
    public HackableSource hackableSource; 
    public CanvasGroup terminal;
    public GameObject player;
    public GameObject warningIndicator;
    public Image signalFill;
    public Image signalFillGUI;
    public TextMeshProUGUI signalText;
    public TextMeshProUGUI signalTextGUI;
    public float hackRadius;

    private bool losingConnection;

    private void Start()
    {
        signalText.text = "no signal";
        signalTextGUI.text = signalText.text;
        signalFill.fillAmount = 0;
        signalFillGUI.fillAmount = signalFill.fillAmount;
    }

    private void Update()
    {
        if (hackableSource != null)
        {
            CheckSignal();
        }
        else
        {
            signalText.text = "no signal";
            signalTextGUI.text = signalText.text;
            signalFill.fillAmount = 0;
            signalFillGUI.fillAmount = signalFill.fillAmount;
        }
    }

    public void OpenTerminal()
    {
        terminal.alpha = 1;
        terminal.blocksRaycasts = true;
    }

    public void CloseTerminal()
    {
        terminal.alpha = 0;
        terminal.blocksRaycasts = false;
    }

    private void CheckSignal()
    {
        float distance = (player.transform.position - hackableSource.transform.position).magnitude;
        int amount = 0;
        float fillAmount = 0;
        if (distance < hackRadius)
        {
            losingConnection = false;
            signalText.text = "";
            signalTextGUI.text = signalText.text;
            amount = Mathf.RoundToInt((distance * 10) / hackRadius) - 10;
            fillAmount = Mathf.Abs(amount) / 10f;
            signalFill.fillAmount = fillAmount;
            signalFillGUI.fillAmount = signalFill.fillAmount;
        }
        else
        {
            if (!losingConnection)
            {
                losingConnection = true;
                StartCoroutine(ConnectionTimer());
                signalText.text = "out of range";
                signalTextGUI.text = signalText.text;
                signalFill.fillAmount = 0;
                signalFillGUI.fillAmount = signalFill.fillAmount;
            }
        }
    }

    private IEnumerator ConnectionTimer()
    {
        int warningFlashes = 3;
        while (losingConnection)
        {
            warningIndicator.SetActive(true);
            yield return new WaitForSeconds(.5f);
            warningIndicator.SetActive(false);
            warningFlashes--;
            yield return new WaitForSeconds(.5f);

            if (warningFlashes <= 0)
            {
                hackableSource = null;
                signalText.text = "no signal";
                signalTextGUI.text = signalText.text;
                signalFill.fillAmount = 0;
                signalFillGUI.fillAmount = signalFill.fillAmount;
                warningIndicator.SetActive(false);
                losingConnection = false;
            }
        }
    }
}
