using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerminalOS : MonoBehaviour
{
    public CanvasGroup terminalGUI;
    public Terminal workingTerminal;
    public Image loadingBar;
    public GameObject loadingBarWindow;
    public GameObject terminalAccessWindow;
    public GameObject terminalAccessIcon;
    public GameController gameController;

    private void Awake()
    {
        terminalGUI.alpha = 0;
        gameController = FindObjectOfType<GameController>();
    }

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



    public void UseLights()
    {
        workingTerminal.UseLights();
    }
}
