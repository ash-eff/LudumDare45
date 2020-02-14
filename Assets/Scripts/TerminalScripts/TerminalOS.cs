using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerminalOS : MonoBehaviour
{
    public Terminal workingTerminal;
    public Image loadingBar;
    public GameObject loadingBarWindow;
    public GameObject terminalAccessWindow;
    public GameObject terminalAccessIcon;
    public GameObject[] minimaps;
    public TextMeshProUGUI[] noFeedTexts;

    public void AttachTerminal(Terminal _terminal)
    {
        workingTerminal = _terminal;
    }

    public void CloseTerminalAccessWindow()
    {
        terminalAccessWindow.SetActive(false);
    }

    public void OpenTerminalAccessWindow()
    {
        terminalAccessWindow.SetActive(true);
    }

    public void ResetOS()
    {
        loadingBar.fillAmount = 0;
        loadingBar.transform.parent.gameObject.SetActive(true);
        terminalAccessWindow.SetActive(false);
        terminalAccessIcon.SetActive(false);
    }

    public void UnlockDoors()
    {
        foreach(Door door in workingTerminal.doors)
        {
            if (door.IsLocked)
            {
                door.IsLocked = false;
            }
        }
    }

    public void ActivateMinimaps()
    {
        foreach(GameObject go in minimaps)
        {
            go.SetActive(true);
        }

        foreach(TextMeshProUGUI tmp in noFeedTexts)
        {
            tmp.gameObject.SetActive(false);
        }
    }

    public void UseLights()
    {
        workingTerminal.UseLights();
    }
}
