using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject interactPanel;
    public Image interactTimeFill;

    private bool isSpotted;
    private bool canInteract;
    private int totalMoneyStolen;
    private GameController gameController;
    private GameObject currentItemBeingInteractedWith;

    public bool IsSpotted { get { return isSpotted; } }
    public bool CanInteract { get { return canInteract; } }

    private void Start()
    {
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
        Debug.Log(totalMoneyStolen);
        Destroy(currentItemBeingInteractedWith);
        currentItemBeingInteractedWith = null;
        gameController.CalculatePlayerScore(totalMoneyStolen);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            interactPanel.SetActive(true);
            currentItemBeingInteractedWith = collision.gameObject;
            canInteract = true;
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
    }
}
