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
    public Image signalFill;
    public Image signalGUIIcon;
    public Sprite[] signalSprites;
    public TextMeshProUGUI signalText;
    public float hackRadius;

    private void Start()
    {
        signalGUIIcon.sprite = signalSprites[6];
        signalText.text = "- no signal -";
        signalFill.fillAmount = 0;
    }

    private void Update()
    {
        if (hackableSource != null)
        {
            CheckSignal();
        }
        else
        {
            signalGUIIcon.sprite = signalSprites[6];
            signalText.text = "- no signal -";
            signalFill.fillAmount = 0;
        }
    }

    private void CheckSignal()
    {
        float distance = (player.transform.position - hackableSource.transform.position).magnitude;
        int amount = 0;
        float fillAmount = 0;
        if (distance < hackRadius)
        {
            signalText.text = "";
            amount = Mathf.RoundToInt((distance * 10) / hackRadius) - 10;
            CheckSignalIcon(Mathf.Abs(amount));
            fillAmount = Mathf.Abs(amount) / 10f;
            signalFill.fillAmount = fillAmount;
        }
        else
        {
            signalGUIIcon.sprite = signalSprites[0];
            signalFill.fillAmount = 0;
            signalText.text = "- out of range -";
        }
    }

    void CheckSignalIcon(int val)
    {
        Debug.Log(val);
        if(val >= 9)
        {
            signalGUIIcon.sprite = signalSprites[5];
        }
        else if (val >= 7)
        {
            signalGUIIcon.sprite = signalSprites[4];
        }
        else if (val >= 5)
        {
            signalGUIIcon.sprite = signalSprites[3];
        }
        else if (val >= 3)
        {
            signalGUIIcon.sprite = signalSprites[2];
        }
        else if (val >= 1)
        {
            signalGUIIcon.sprite = signalSprites[1];
        }
        else
        {
            signalGUIIcon.sprite = signalSprites[0];
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
}
