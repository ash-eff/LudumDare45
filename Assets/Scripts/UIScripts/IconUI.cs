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

    public void OpenConnectWindow()
    {
        if (!window.gameObject.activeInHierarchy)
        {
            if (hackTest.hackableSource != null)
            {
                windowText.text = "Connect to host " + hackTest.hackableSource.consoleName;
                window.gameObject.SetActive(true);
            }
            else if (hackTest.hackableSource == null)
            {
                windowText.text = "No Host to connect to... " + hackTest.hackableSource.consoleName;
                window.gameObject.SetActive(true);
            }
            else
            {
                windowText.text = "Already connected to host " + hackTest.hackableSource.consoleName;
                window.gameObject.SetActive(true);
            }
        }
    }

    public void OpenCrackWindow()
    {
        if (!window.gameObject.activeInHierarchy)
        {
            if (hackTest.hackableSource != null)
            {
                windowText.text = "Crack pinpad?";
                window.gameObject.SetActive(true);
            }
        }
    }

    public void OpenHackBox()
    {
        if (!window.gameObject.activeInHierarchy)
        {
            if (hackTest.hackableSource != null)
            {
                windowText.text = "Hackbox v 1.7";
                window.gameObject.SetActive(true);
            }
        }
    }

    public void OpenDesktopWindow()
    {
        if (!window.gameObject.activeInHierarchy)
        {
            if (hackTest.hackableSource != null)
            {
                if (!hackTest.hackableSource.unlocked)
                {
                    StartCoroutine(AccessFlash("ACcess Denied."));
                }
                else
                {
                    StartCoroutine(AccessFlash("ACcess Granted."));
                }

            }
        }
    }

    IEnumerator AccessFlash(string accessText)
    {
        windowText.text = accessText;
        window.gameObject.SetActive(true);
        yield return new WaitForSeconds(.25f);
        windowText.text = "";
        yield return new WaitForSeconds(.25f);
        windowText.text = accessText;
        yield return new WaitForSeconds(.25f);
        windowText.text = "";
        yield return new WaitForSeconds(.25f);
        windowText.text = accessText;
        yield return new WaitForSeconds(.25f);
        window.gameObject.SetActive(false);
    }

    public void OpenBruteWindow()
    {
        if (!window.gameObject.activeInHierarchy)
        {
            if (hackTest.hackableSource != null)
            {
                windowText.text = "Brute Force";
                window.gameObject.SetActive(true);
                WindowUI hackboxWindow = GetComponentInParent<WindowUI>();
                hackTest.BruteForce(window.GetComponent<ProgressBarUI>().progressBar, window, hackboxWindow);
            }
        }
    }
}
