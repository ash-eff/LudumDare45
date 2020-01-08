using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockPad : MonoBehaviour
{
    public Lock currentLock;
    public TextMeshProUGUI cheatText;
    public Image[] digits;
    public Image pinPad;
    public Image fillBar;
    public Image lights;
    public Image wrong;
    public Sprite[] availableDigitSprites;
    public Sprite defaultDigitSprite;
    public Sprite[] availablePinPadSprites;
    public Sprite[] availableLightSprites;
    public Sprite[] availableWrongSprites;
    public AudioSource audioSource;
    public AudioClip button, success, failure, correct, incorrect;
    public int successfulHacks;

    public Color[] colors;

    KeyCode[] availableKeys = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
                                KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    private int digitIndex = 0;
    private string codeAttempt;
    public List<int> lockCode;

    private void OnEnable()
    {

        if (currentLock != null)
        {
            cheatText.text = "cheat code: " + currentLock.lockCode;
            lockCode = new List<int>();
            lockCode.Add(currentLock.lockCode[0] - '0');
            lockCode.Add(currentLock.lockCode[1] - '0');
            lockCode.Add(currentLock.lockCode[2] - '0');
            lockCode.Add(currentLock.lockCode[3] - '0');
        }
        ResetPinPad();
    }

    private void OnDisable()
    {
        if(currentLock != null)
        {
            currentLock = null;
            cheatText.text = "0000";
        }
    }

    private void Update()
    {
        if(digitIndex < 4)
        {
            foreach (KeyCode key in availableKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    audioSource.PlayOneShot(button);
                    switch (key)
                    {
                        case KeyCode.Alpha0:
                            digits[digitIndex].sprite = availableDigitSprites[0];
                            pinPad.sprite = availablePinPadSprites[0];
                            codeAttempt += "0";
                            break;

                        case KeyCode.Alpha1:
                            digits[digitIndex].sprite = availableDigitSprites[1];
                            pinPad.sprite = availablePinPadSprites[1];
                            codeAttempt += "1";
                            break;

                        case KeyCode.Alpha2:
                            digits[digitIndex].sprite = availableDigitSprites[2];
                            pinPad.sprite = availablePinPadSprites[2];
                            codeAttempt += "2";
                            break;

                        case KeyCode.Alpha3:
                            digits[digitIndex].sprite = availableDigitSprites[3];
                            pinPad.sprite = availablePinPadSprites[3];
                            codeAttempt += "3";
                            break;

                        case KeyCode.Alpha4:
                            digits[digitIndex].sprite = availableDigitSprites[4];
                            pinPad.sprite = availablePinPadSprites[4];
                            codeAttempt += "4";
                            break;

                        case KeyCode.Alpha5:
                            digits[digitIndex].sprite = availableDigitSprites[5];
                            pinPad.sprite = availablePinPadSprites[5];
                            codeAttempt += "5";
                            break;

                        case KeyCode.Alpha6:
                            digits[digitIndex].sprite = availableDigitSprites[6];
                            pinPad.sprite = availablePinPadSprites[6];
                            codeAttempt += "6";
                            break;

                        case KeyCode.Alpha7:
                            digits[digitIndex].sprite = availableDigitSprites[7];
                            pinPad.sprite = availablePinPadSprites[7];
                            codeAttempt += "7";
                            break;

                        case KeyCode.Alpha8:
                            digits[digitIndex].sprite = availableDigitSprites[8];
                            pinPad.sprite = availablePinPadSprites[8];
                            codeAttempt += "8";
                            break;

                        case KeyCode.Alpha9:
                            digits[digitIndex].sprite = availableDigitSprites[9];
                            pinPad.sprite = availablePinPadSprites[9];
                            codeAttempt += "9";
                            break;
                    }

                    digitIndex++;

                    if(digitIndex > 3)
                    {
                        CheckCode();
                    }
                }
            }
        }
    }

    private void CheckCode()
    {
        if(codeAttempt == currentLock.lockCode)
        {
            StartCoroutine(Success());
        }
        else
        {
            StartCoroutine(Failure());
        }
    }

    private void ResetPinPad()
    {
        fillBar.color = colors[0];
        lights.sprite = availableLightSprites[0];
        wrong.sprite = availableWrongSprites[0];
        pinPad.sprite = availablePinPadSprites[10];
        digits[0].sprite = availableDigitSprites[0];
        digits[1].sprite = availableDigitSprites[0];
        digits[2].sprite = availableDigitSprites[0];
        digits[3].sprite = availableDigitSprites[0];
        digitIndex = 0;
        codeAttempt = "";
        StartCoroutine(HackPinPad());
    }

    IEnumerator Success()
    {
        yield return new WaitForSeconds(.1f);
        digits[0].sprite = availableDigitSprites[10];
        digits[1].sprite = availableDigitSprites[11];
        digits[2].sprite = availableDigitSprites[12];
        digits[3].sprite = availableDigitSprites[14];
        audioSource.PlayOneShot(success);
        pinPad.sprite = availablePinPadSprites[12];
        yield return new WaitForSeconds(.1f);
        pinPad.sprite = availablePinPadSprites[10];
        yield return new WaitForSeconds(.1f);
        audioSource.PlayOneShot(success);
        pinPad.sprite = availablePinPadSprites[12];
        yield return new WaitForSeconds(.1f);
        pinPad.sprite = availablePinPadSprites[10];
        yield return new WaitForSeconds(.1f);
        audioSource.PlayOneShot(success);
        pinPad.sprite = availablePinPadSprites[12];
        yield return new WaitForSeconds(.25f);
        currentLock.Unlock();
        this.gameObject.SetActive(false);
    }

    IEnumerator Failure()
    {
        yield return new WaitForSeconds(.1f);
        audioSource.PlayOneShot(failure);
        digits[0].sprite = availableDigitSprites[13];
        digits[1].sprite = availableDigitSprites[11];
        digits[2].sprite = availableDigitSprites[10];
        digits[3].sprite = availableDigitSprites[14];
        pinPad.sprite = availablePinPadSprites[11];
        yield return new WaitForSeconds(.1f);
        pinPad.sprite = availablePinPadSprites[10];
        yield return new WaitForSeconds(.1f);
        pinPad.sprite = availablePinPadSprites[11];
        yield return new WaitForSeconds(.1f);
        pinPad.sprite = availablePinPadSprites[10];
        yield return new WaitForSeconds(.1f);
        pinPad.sprite = availablePinPadSprites[11];
        yield return new WaitForSeconds(.25f);
        ResetPinPad();
    }

    IEnumerator HackPinPad()
    {
        float hackSpeed = .5f;
        float hackPos = 0f;
        int totalFailures = 3;
        successfulHacks = 0;
        while(successfulHacks < 4)
        {
            float t = Mathf.PingPong(Time.time * hackSpeed, 1f);
            fillBar.fillAmount = t;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                hackPos = fillBar.fillAmount;
                if (hackPos > .69f && hackPos < .83f)
                {
                    audioSource.PlayOneShot(correct);
                    digits[successfulHacks].sprite = availableDigitSprites[lockCode[successfulHacks]];
                    successfulHacks++;
                    if(successfulHacks < 4)
                    {
                        lights.sprite = availableLightSprites[successfulHacks];
                        fillBar.color = colors[successfulHacks];
                    }

                    hackSpeed += .25f;
                    t = 0f;
                    fillBar.fillAmount = t;
                }
                else
                {
                    audioSource.PlayOneShot(incorrect);
                    wrong.sprite = availableWrongSprites[totalFailures];
                    totalFailures--;                   
                    if (totalFailures == 0)
                    {
                        StartCoroutine(Failure());
                        yield break;
                    }
                }
            }

            yield return null;
        }

        StartCoroutine(Success());
    }
}
