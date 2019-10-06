using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public GameObject mallDirectory;
    public GameObject interactPanel;
    public GameObject deathPanel;
    public TextMeshProUGUI stolenValueText;
    public Image interactTimeFill;

    private bool isSpotted;
    private bool canInteract;
    private int totalMoneyStolen;
    private GameController gameController;
    private GameObject currentItemBeingInteractedWith;
    private SpriteRenderer spr;

    public bool IsSpotted { get { return isSpotted; } }
    public bool CanInteract { get { return canInteract; } }
    public int TotalMoneyStolen { get { return totalMoneyStolen; } }

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        stolenValueText.text = "Money stolen: $0000";
        gameController = FindObjectOfType<GameController>();
    }

    public void PlayerSpotted()
    {
        isSpotted = true;
        canInteract = false;
        gameController.GameLost();
    }

    public void StealMoney()
    {
        canInteract = false;
        totalMoneyStolen += currentItemBeingInteractedWith.GetComponent<ValuableItem>().itemValue;
        Destroy(currentItemBeingInteractedWith);
        currentItemBeingInteractedWith = null;
        UpdateMoneyStolenValue(totalMoneyStolen);
    }

    public void UpdateMoneyStolenValue(int value)
    {
        stolenValueText.text = "Money Stolen: $" + value.ToString("0000");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            interactPanel.SetActive(true);
            currentItemBeingInteractedWith = collision.gameObject;
            canInteract = true;
        }

        if(collision.tag == "Directory")
        {
            mallDirectory.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            interactPanel.SetActive(false);
            currentItemBeingInteractedWith = null;
            canInteract = false;
        }

        if (collision.tag == "Directory")
        {
            mallDirectory.SetActive(false);
        }
    }

    public void Kill()
    {
        spr.enabled = false;
        deathPanel.SetActive(true);
    }
}
