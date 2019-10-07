using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public GameObject mallDirectory;
    public GameObject textInformationPanel;
    public TextMeshProUGUI textInformation;
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI storeText;
    public GameObject deathPanel;
    public GameObject hideActionsPanel;
    public TextMeshProUGUI stolenValueText;
    public GameObject interactPanel;
    public Image interactTimeFill;

    private bool isSpotted;
    private bool canInteract;
    private bool canTeleport;
    private int totalMoneyStolen;
    private GameController gameController;
    private GameObject currentItemBeingInteractedWith;
    private SpriteRenderer spr;
    private PlayerController playercontroller;
    private Transporter currentTransporter;
    private Animator anim;

    public bool IsSpotted { get { return isSpotted; } }
    public bool CanInteract { get { return canInteract; } }
    public bool CanTeleport { get { return canTeleport; } }
    public int TotalMoneyStolen { get { return totalMoneyStolen; } }
    public Transporter CurrentTransporter { get { return currentTransporter; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playercontroller = GetComponent<PlayerController>();
        transform.position = FindObjectOfType<StartPoint>().transform.position;
        spr = GetComponent<SpriteRenderer>();
        stolenValueText.text = "Money stolen: $0000";
        gameController = FindObjectOfType<GameController>();
        CheckFloor();
        StartCoroutine(SetPlayerUntargetable(2f));
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

    public IEnumerator HidePlayerTeleport()
    {
        hideActionsPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        hideActionsPanel.SetActive(false);
    }

    public IEnumerator SetPlayerUntargetable(float untargetableTime)
    {
        anim.SetBool("isUntargetable", true);
        gameObject.layer = 10;
        yield return new WaitForSecondsRealtime(untargetableTime);
        anim.SetBool("isUntargetable", false);
        gameObject.layer = 9;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            DisplayTextInformation("press 'e' to steal!");
            currentItemBeingInteractedWith = collision.gameObject;
            canInteract = true;
        }

        if(collision.tag == "Directory")
        {
            mallDirectory.SetActive(true);
        }

        if (collision.tag == "Transporter" && !canTeleport)
        {
            currentTransporter = collision.GetComponent<Transporter>();
            string teleporterInfoText = collision.GetComponent<Transporter>().transporterInfo;
            DisplayTextInformation(teleporterInfoText);
            canTeleport = true;
        }

        if (collision.tag == "Store")
        {
            storeText.text = collision.GetComponent<Store>().storeName;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            CancelTextInformation();
            currentItemBeingInteractedWith = null;
            canInteract = false;
        }

        if (collision.tag == "Directory")
        {
            mallDirectory.SetActive(false);
        }

        if (collision.tag == "Transporter")
        {
            currentTransporter = null;
            CancelTextInformation();
            canTeleport = false;
        }
    }

    public void DisplayTextInformation(string s)
    {
        textInformation.text = s;
        textInformationPanel.SetActive(true);
    }

    public void CancelTextInformation()
    {     
        textInformationPanel.SetActive(false);
        textInformation.text = "";
    }

    public void Kill()
    {
        spr.enabled = false;
        deathPanel.SetActive(true);
    }

    public void CheckFloor()
    {
        if(transform.position.x < 75f)
        {
            floorText.text = "3rd Floor";
        }

        if (transform.position.x > 75f && transform.position.x < 250f)
        {
            floorText.text = "2nd Floor";
        }

        if (transform.position.x > 250f)
        {
            floorText.text = "1st Floor";
        }
    }
}
