﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HackingTest : MonoBehaviour
{
    //public Terminal terminal; 
    //
    //public GameObject player;
    //public GameObject warningIndicator;
    //public Image signalFill;
    //public Image signalFillGUI;
    //public TextMeshProUGUI signalText;
    //public TextMeshProUGUI signalTextGUI;
    //public TextMeshProUGUI terminalText;
    //public int outOfRangeChances = 3;
    //public float baseHackTime;
    //public WindowUI currentWindow;
    //public WindowUI defaultWindow;
    //public Door[] doors;
    //
    //public float hackRadius;
    //
    //public bool isConnectedToHost = false;
    //private bool losingConnection;
    //
    //private float currentXPos;
    //
    //private void Start()
    //{
    //    //currentXPos = transform.localPosition.x;
    //    signalText.text = "no signal";
    //    signalTextGUI.text = signalText.text;
    //    signalFill.fillAmount = 0;
    //    signalFillGUI.fillAmount = signalFill.fillAmount;
    //    StartCoroutine(TypeOutText("No target host available..."));
    //}
    //
    //private void Update()
    //{
    //    //transform.localPosition = new Vector2(currentXPos * player.transform.localScale.x, 0f);
    //}
    //
    //public void ConnectToHost()
    //{
    //    StartCoroutine(IEConnectToHost());
    //}
    //
    //public void CrackCode()
    //{
    //    StartCoroutine(IECrackCode());
    //}
    //
    //public void BruteForce(Image bar, WindowUI win, WindowUI parentWin)
    //{
    //    StartCoroutine(IEBruteForce(bar, win, parentWin));
    //}
    //
    //public void HostAvailable()
    //{
    //    StartCoroutine(TypeOutText(terminal.consoleName + " available as host."));
    //}
    //
    //public void NoHost()
    //{
    //    StartCoroutine(TypeOutText("No available host in range."));
    //}
    //
    //IEnumerator IEConnectToHost()
    //{
    //    if(terminal != null)
    //    {
    //        if (!isConnectedToHost)
    //        {
    //            yield return StartCoroutine(TypeOutText("Host manufacturer: " + terminal.manufacturersInformation));
    //            yield return StartCoroutine(TypeOutText("Host difiiculty rating " + terminal.difficultyRating));
    //            yield return StartCoroutine(TypeOutText("Connection successful..."));
    //            isConnectedToHost = true;
    //            terminal.window.gameObject.SetActive(true);
    //            StartCoroutine(ManageConnection());
    //        }
    //        else
    //        {
    //            yield return StartCoroutine(TypeOutText("Already Connected to " + terminal.consoleName));
    //        }
    //    }
    //    else
    //    {
    //        yield return StartCoroutine(TypeOutText("No target host available..."));
    //    }
    //}
    //
    //IEnumerator IECrackCode()
    //{
    //    float crackTime = 2f;
    //    TextMeshProUGUI tempText = terminal.window.GetComponentInChildren<TextMeshProUGUI>();
    //    while (crackTime > 0)
    //    {
    //        int rand = Random.Range(1, 9999);
    //        tempText.text = rand.ToString("0000");
    //        yield return null;
    //        crackTime -= Time.deltaTime;
    //    }
    //
    //    isConnectedToHost = false;
    //    tempText.text = terminal.GetComponent<Lock>().lockCode;
    //    yield return new WaitForSeconds(1f);
    //    terminal.window.gameObject.SetActive(false);
    //    terminal.GetComponent<Lock>().Unlock();
    //    terminal = null;
    //}
    //
    //IEnumerator IEBruteForce(Image bar, WindowUI win, WindowUI parentWindow)
    //{
    //    int multiplier = terminal.difficultyRating.Length;
    //    float hackTime = baseHackTime * multiplier;
    //    Image fillBar = bar;
    //    while (isConnectedToHost)
    //    {
    //        fillBar.fillAmount += Time.deltaTime / hackTime;
    //        yield return null;
    //        if(fillBar.fillAmount >= 1)
    //        {
    //            break;
    //        }
    //    }
    //
    //    win.GetComponentInChildren<TextMeshProUGUI>().text = "Access Granted";
    //    yield return new WaitForSeconds(.5f);
    //    win.gameObject.SetActive(false);
    //    parentWindow.gameObject.SetActive(false);
    //    terminal.unlocked = true;
    //}
    //
    //IEnumerator TypeOutText(string stringText)
    //{
    //    string newString = stringText;
    //    int stringLength = newString.Length;
    //    for(int i = 0; i < stringLength; i++)
    //    {
    //        terminalText.text += newString[i];
    //        yield return null;
    //    }
    //
    //    terminalText.text += "\n";
    //}
    //
    //
    //
    //private IEnumerator ManageConnection()
    //{
    //    float distance = (player.transform.position - terminal.transform.position).magnitude;
    //    int amount = 0;
    //    float fillAmount = 0;
    //    int warningFlashes = outOfRangeChances;
    //    while (isConnectedToHost)
    //    {
    //        distance = (player.transform.position - terminal.transform.position).magnitude;
    //        if (distance < hackRadius)
    //        {               
    //            warningIndicator.SetActive(false);
    //            losingConnection = false;
    //            warningFlashes = outOfRangeChances;
    //            signalText.text = "";
    //            signalTextGUI.text = signalText.text;
    //            amount = Mathf.RoundToInt((distance * 10) / hackRadius) - 10;
    //            fillAmount = Mathf.Abs(amount) / 10f;
    //            signalFill.fillAmount = fillAmount;
    //            signalFillGUI.fillAmount = signalFill.fillAmount;
    //        }
    //        else
    //        {
    //            if (!losingConnection)
    //            {
    //                losingConnection = true;
    //                signalText.text = "out of range";
    //                signalTextGUI.text = signalText.text;
    //                signalFill.fillAmount = 0;
    //                signalFillGUI.fillAmount = signalFill.fillAmount;
    //            }
    //        }
    //
    //        if (losingConnection)
    //        {
    //            warningIndicator.SetActive(true);
    //            yield return new WaitForSeconds(.5f);
    //            warningIndicator.SetActive(false);
    //            warningFlashes--;
    //            yield return new WaitForSeconds(.5f);
    //
    //            if (warningFlashes <= 0)
    //            {
    //                terminal = null;
    //                signalText.text = "no signal";
    //                signalTextGUI.text = signalText.text;
    //                signalFill.fillAmount = 0;
    //                signalFillGUI.fillAmount = signalFill.fillAmount;
    //                warningIndicator.SetActive(false);
    //                losingConnection = false;
    //                isConnectedToHost = false;
    //                StartCoroutine(TypeOutText("Connection lost..."));
    //            }
    //        }
    //
    //        yield return null;
    //    }
    //   
    //}
    //
    //public void UnlockDoors()
    //{
    //    foreach (Door door in doors)
    //    {
    //        if (door.locked)
    //        {
    //            door.locked = false;
    //        }
    //    }
    //}
}
