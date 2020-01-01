using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public string lockCode;

    void Start()
    {
        int firstDigit = Random.Range(0, 10);
        int secondDigit = Random.Range(0, 10);
        int thirdDigit = Random.Range(0, 10);
        int fourthDigit = Random.Range(0, 10);
        lockCode = firstDigit.ToString() + secondDigit.ToString() + thirdDigit.ToString() + fourthDigit.ToString();
    }
}
