using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : HackableSource
{
    public GameObject lockImg, barrier, exitSign;
    public string lockCode;

    void Start()
    {
        int firstDigit = Random.Range(0, 10);
        int secondDigit = Random.Range(0, 10);
        int thirdDigit = Random.Range(0, 10);
        int fourthDigit = Random.Range(0, 10);
        lockCode = firstDigit.ToString() + secondDigit.ToString() + thirdDigit.ToString() + fourthDigit.ToString();
    }

    public void Unlock()
    {
        lockImg.SetActive(false);
        barrier.SetActive(false);
        exitSign.SetActive(true);
    }
}
