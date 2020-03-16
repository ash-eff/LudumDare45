using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.PlayerController;
using TMPro;

public class TerminalOS : MonoBehaviour
{
    public CanvasGroup terminalGUI;
    public Terminal workingTerminal;
    public Image loadingBar;
    public Image securityBar;
    public TextMeshProUGUI loadingText;
    public GameObject securityGranted;
    public GameObject loadingBarWindow;
    public GameObject securityBarWindow;
    public GameObject terminalAccessWindow;
    public GameObject terminalAccessIcon;
    public GameObject securityAccessIcon;
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
        loadingBar.transform.parent.gameObject.SetActive(false);
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

    public void SecurityIcon()
    {
        terminalAccessWindow.SetActive(false);
        StartCoroutine(LoadAccess("Accessing Security", securityAccessIcon));
    }

    public void SecuritySystemAccess()
    {
        StartCoroutine(FillSecurityBar());
    }

    IEnumerator FillSecurityBar()
    {
        securityBar.fillAmount = 0;
        securityBarWindow.SetActive(true);
        while (securityBar.fillAmount < 1)
        {
            securityBar.fillAmount += Time.deltaTime;
            yield return null;
        }

        securityGranted.SetActive(true);
        yield return new WaitForSeconds(.5f);
        securityGranted.SetActive(false);
        securityBarWindow.SetActive(false);
        PlayerController player = FindObjectOfType<PlayerController>();
        player.TargetRobots(true);
    }

    IEnumerator LoadAccess(string accessText, GameObject icon)
    {
        bool doneLoading = false;
        loadingBar.fillAmount = 0;
        loadingText.text = "";

        while (!doneLoading)
        {
            doneLoading = GetLoadAccess(workingTerminal, accessText);

            yield return null;
        }

        icon.SetActive(true);
    }

    public bool GetLoadAccess(Terminal _terminal, string message)
    {
        loadingBarWindow.SetActive(true);
        loadingText.text = message;
        loadingBar.fillAmount += Time.deltaTime;
        if (loadingBar.fillAmount < 1)
        {
            return false;
        }

        loadingBarWindow.SetActive(false);
        return true;
    }
}
