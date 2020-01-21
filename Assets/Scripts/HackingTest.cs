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
    public TextMeshProUGUI terminalText;
    public int outOfRangeChances = 3;
    public WindowUI currentWindow;
    public WindowUI defaultWindow;

    public float hackRadius;

    public bool isConnectedToHost = false;
    private bool losingConnection;

    private void Start()
    {
        signalText.text = "no signal";
        signalTextGUI.text = signalText.text;
        signalFill.fillAmount = 0;
        signalFillGUI.fillAmount = signalFill.fillAmount;
        StartCoroutine(TypeOutText("No target host..."));
    }

    public void ConnectToHost()
    {
        StartCoroutine(IEConnectToHost());
    }

    IEnumerator IEConnectToHost()
    {
        if (!isConnectedToHost)
        {
            yield return StartCoroutine(TypeOutText("Connecting to " + hackableSource.consoleName));
            yield return StartCoroutine(TypeOutText("Host manufacturer: " + hackableSource.manufacturersInformation));
            yield return StartCoroutine(TypeOutText("Host difiiculty rating" + hackableSource.difficultyRating));
            yield return StartCoroutine(TypeOutText("Connection successful..."));
            isConnectedToHost = true;
            StartCoroutine(ManageConnection());
        }
        else
        {
            yield return StartCoroutine(TypeOutText("Already Connected to " + hackableSource.consoleName));
        }

    }

    IEnumerator TypeOutText(string stringText)
    {
        string newString = stringText;
        int stringLength = newString.Length;
        for(int i = 0; i < stringLength; i++)
        {
            terminalText.text += newString[i];
            yield return null;
        }

        terminalText.text += "\n";
    }

    public void OpenHackBox()
    {
        if (!isConnectedToHost)
        {
            currentWindow = defaultWindow;
        }
        else
        {
            currentWindow = hackableSource.window;
        }

        currentWindow.gameObject.SetActive(true);
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

    private IEnumerator ManageConnection()
    {
        float distance = (player.transform.position - hackableSource.transform.position).magnitude;
        int amount = 0;
        float fillAmount = 0;
        int warningFlashes = outOfRangeChances;
        while (isConnectedToHost)
        {
            distance = (player.transform.position - hackableSource.transform.position).magnitude;
            if (distance < hackRadius)
            {               
                warningIndicator.SetActive(false);
                losingConnection = false;
                warningFlashes = outOfRangeChances;
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
                    signalText.text = "out of range";
                    signalTextGUI.text = signalText.text;
                    signalFill.fillAmount = 0;
                    signalFillGUI.fillAmount = signalFill.fillAmount;
                }
            }

            if (losingConnection)
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
                    isConnectedToHost = false;
                    StartCoroutine(TypeOutText("Connection lost..."));
                }
            }

            yield return null;
        }
       
    }
}
