using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IconUI : MonoBehaviour
{
    public HackingTest hackTest;
    public WindowUI window;
    public TextMeshProUGUI windowText;

    public void OpenWindow()
    {
        if (!window.gameObject.activeInHierarchy)
        {
            if (hackTest.hackableSource != null)
            {
                windowText.text = "Connect to host " + hackTest.hackableSource.consoleName;
                window.gameObject.SetActive(true);
            }
            else
            {
                windowText.text = "Already connected to host " + hackTest.hackableSource.consoleName;
                window.gameObject.SetActive(true);
            }
        }
    }
}
