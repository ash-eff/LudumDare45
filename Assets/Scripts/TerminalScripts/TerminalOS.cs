using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalOS : MonoBehaviour
{
    public Image loadingBar;
    public GameObject terminalAccessWindow;
    public GameObject terminalAccessIcon;

    public void CloseTerminalWindow()
    {
        terminalAccessWindow.SetActive(false);
    }

    public void OpenTerminalWindow()
    {
        terminalAccessWindow.SetActive(true);
    }
}
