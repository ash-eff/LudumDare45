using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockPad : MonoBehaviour
{
    public Lock currentLock;
    public TextMeshProUGUI cheatText;
    public SpriteRenderer[] digits;
    public SpriteRenderer pinPad;
    public Sprite[] availableDigitSprites;
    public Sprite defaultDigitSprite;
    public Sprite[] availablePinPadSprites;
    public AudioSource audioSource;
    public AudioClip button;
    public AudioClip success;
    public AudioClip failure;

    KeyCode[] availableKeys = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
                                KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    private int digitIndex = 0;
    private string codeAttempt;

    private void OnEnable()
    {
        if(currentLock != null)
        {
            cheatText.text = "cheat code: " + currentLock.lockCode;
        }
    }

    private void OnDisable()
    {
        if(currentLock != null)
        {
            currentLock = null;
            cheatText.text = "0000";
            ResetPinPad();
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
        pinPad.sprite = availablePinPadSprites[10];
        digits[0].sprite = availableDigitSprites[0];
        digits[1].sprite = availableDigitSprites[0];
        digits[2].sprite = availableDigitSprites[0];
        digits[3].sprite = availableDigitSprites[0];
        digitIndex = 0;
        codeAttempt = "";
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
        currentLock.gameObject.SetActive(false);
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
}
