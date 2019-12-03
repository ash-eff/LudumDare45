using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public LayerMask wallLayer;
    public float xRaylength;
    public float yRaylength;
    public float xOffset;
    public float yOffset;
    public float xNumOfRays;
    public float yNumOfRays;
    public float stealTime;
    public float moveSpeed;
    public int lives = 3;


    //
    //public GameObject mallDirectory;
    //public GameObject textInformationPanel;
    //public TextMeshProUGUI textInformation;
    //public TextMeshProUGUI floorText;
    //public TextMeshProUGUI storeText;
    //public GameObject deathPanel;
    //public GameObject hideActionsPanel;
    //public TextMeshProUGUI stolenValueText;
    //public TextMeshProUGUI livesTotalText;
    public GameObject interactPanel;
    public Image interactTimeFill;
    //public GameObject firstFloorDir;
    //public GameObject secondFloorDir;
    //public GameObject thirdFloorDir;
    //public AudioClip lootGrab;
    //public AudioSource audioSource;
    //public Vector2 checkPoint;

    public bool playerOccupied;
    public bool isTeleporting;
    public bool isSpotted;
    public bool canInteract;
    public bool canTeleport;
    public bool isTargetable;
    private int totalMoneyStolen;
    private bool isDead;

    private Animator anim;
    private Vector3 movement;
    private GameController gameController;
    private GameObject currentItemBeingInteractedWith;
    public GameObject currentHeldItem;
    private SpriteRenderer spr;
    //private Transporter currentTransporter;
    //private MenuController menuController;

    public bool IsTeleporting { get { return isTeleporting; } }
    public bool IsSpotted { get { return isSpotted; } }
    public bool IsDead { get { return isDead; } }
    public int TotalMoneyStolen { get { return totalMoneyStolen; } }

    private void Awake()
    {
        //menuController = FindObjectOfType<MenuController>();
        anim = GetComponent<Animator>();
        transform.position = transform.position;
        spr = GetComponent<SpriteRenderer>();
        //stolenValueText.text = "Money stolen: $0000";
        //livesTotalText.text = "Lives: " + lives.ToString();
        gameController = FindObjectOfType<GameController>();
        //CheckCurrentFloor();
        //checkPoint = transform.position;
        //StartCoroutine(SetPlayerUntargetable(2f));
    }

    private void Update()
    {
        if (isSpotted || playerOccupied || gameController.IsGameOver)
        {
            return;
        }

        Vector3 castPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0f);
        movement = Vector3.zero;
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        Debug.DrawRay(castPosition, Vector3.up * yRaylength, Color.red);
        Debug.DrawRay(castPosition, -Vector3.up * yRaylength, Color.green);
        Debug.DrawRay(castPosition, Vector3.right * xRaylength, Color.blue);
        Debug.DrawRay(castPosition, -Vector3.right * xRaylength, Color.yellow);
        if (movement.y != 0)
        {
            if (Physics2D.Raycast(castPosition, Vector3.up, yRaylength, wallLayer) && movement.y == 1f)
            {
                movement.y = 0f;
            }
            if (Physics2D.Raycast(castPosition, -Vector3.up, yRaylength, wallLayer) && movement.y == -1f)
            {
                movement.y = 0f;
            }
        }
        
        if(movement.x != 0)
        {
            if (Physics2D.Raycast(castPosition, Vector3.right, xRaylength, wallLayer) && movement.x == 1f)
            {
                movement.x = 0f;
            }
            if (Physics2D.Raycast(castPosition, -Vector3.right, xRaylength, wallLayer) && movement.x == -1f)
            {
                movement.x = 0f;
            }
        }


        //CheckForButtonPress();
        MovePlayer(movement.normalized);
        //audioSource.volume = menuController.SFXVolume;
    }

    private void FixedUpdate()
    {
        if (isSpotted || playerOccupied || gameController.IsGameOver)
        {
            return;
        }


        //MovePlayer(movement.normalized);
    }

    private void MovePlayer(Vector3 direction)
    {
        transform.position = new Vector3(transform.position.x + (direction.x * moveSpeed * Time.deltaTime),
                                         transform.position.y + (direction.y * moveSpeed * Time.deltaTime), 0f);
    }

    private void CheckForButtonPress()
    {
        if (canInteract && !playerOccupied && isTargetable)
        {
            if (Input.GetButtonDown("Interact"))
            {
                playerOccupied = true;
                StartCoroutine(FillStealIndicatorBar());
            }
        }

        if (currentHeldItem != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropMoney();
            }
        }
    
        //if (canTeleport && !playerOccupied && isTargetable)
        //{
        //    if (Input.GetButtonDown("Interact"))
        //    {
        //        CancelTextInformation();
        //        playerOccupied = true;
        //        StartCoroutine(MovePlayerToNewLocation(currentTransporter.exitLocation.transform.position));
        //        checkPoint = currentTransporter.exitLocation.transform.position;
        //    }
        //}
    }

    private IEnumerator FillStealIndicatorBar()
    {
        interactPanel.SetActive(true);
        float timeOfInteraction = stealTime;
        interactTimeFill.fillAmount = 0f;
        while (Input.GetButton("Interact"))
        {
            timeOfInteraction -= Time.deltaTime;
            interactTimeFill.fillAmount += Time.deltaTime / stealTime;
    
            if (timeOfInteraction <= 0)
            {
                playerOccupied = false;
                StealMoney();
                break;
            }
    
            yield return null;
        }
    
        interactPanel.SetActive(false);
        interactTimeFill.fillAmount = 0f;
        playerOccupied = false;
    }

    private void DropMoney()
    {
        currentHeldItem.GetComponent<ValuableItem>().DropItem(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        currentHeldItem = null;
    }

    private void StealMoney()
    {
        currentHeldItem = currentItemBeingInteractedWith;
        canInteract = false;
        currentItemBeingInteractedWith.GetComponent<ValuableItem>().PickUpItem();    
        currentItemBeingInteractedWith = null;
    }

    //private void StealMoney()
    //{
    //    audioSource.PlayOneShot(lootGrab);
    //    canInteract = false;
    //    totalMoneyStolen += currentItemBeingInteractedWith.GetComponent<ValuableItem>().itemValue;
    //    Destroy(currentItemBeingInteractedWith);
    //    currentItemBeingInteractedWith = null;
    //    UpdateMoneyStolenValue(totalMoneyStolen);
    //}
    //
    //public void UpdateMoneyStolenValue(int value)
    //{
    //    stolenValueText.text = "Money Stolen: $" + value.ToString("0000");
    //}
    //
    //void UpdatesLivesTotal()
    //{
    //    livesTotalText.text = "Lives: " + lives.ToString();
    //}

    //public void PlayerSpotted()
    //{
    //    isSpotted = true;
    //    canInteract = false;
    //    lives--;
    //    //UpdatesLivesTotal();
    //
    //    if (lives > 0)
    //    {
    //        ReturnToCheckPoint();
    //    }
    //    else
    //    {
    //        isDead = true;
    //        gameController.GameLost();
    //    }
    //}

    //private void ReturnToCheckPoint()
    //{
    //    StartCoroutine(MovePlayerToNewLocation(checkPoint));
    //}
    //
    //private IEnumerator MovePlayerToNewLocation(Vector2 location)
    //{
    //    StartCoroutine(SetPlayerUntargetable(4f));
    //    StartCoroutine(HidePlayerTeleport());
    //    yield return new WaitForSecondsRealtime(1f);
    //    isTeleporting = true;
    //    transform.position = location;
    //    CheckCurrentFloor();
    //    yield return new WaitForSecondsRealtime(1.5f);
    //    ResetStatus();
    //}
    //
    //private IEnumerator SetPlayerUntargetable(float untargetableTime)
    //{
    //    isTargetable = false;
    //    anim.SetBool("isUntargetable", true);
    //    gameObject.layer = 10;
    //    yield return new WaitForSecondsRealtime(untargetableTime);
    //    anim.SetBool("isUntargetable", false);
    //    gameObject.layer = 9;
    //    isTargetable = true;
    //}

    //private IEnumerator HidePlayerTeleport()
    //{
    //    hideActionsPanel.SetActive(true);
    //    yield return new WaitForSecondsRealtime(2.5f);
    //    hideActionsPanel.SetActive(false);
    //}
    //
    //private void CheckCurrentFloor()
    //{
    //    if (transform.position.x < 75f)
    //    {
    //        floorText.text = "3rd Floor";
    //        firstFloorDir.SetActive(false);
    //        secondFloorDir.SetActive(false);
    //        thirdFloorDir.SetActive(true);
    //    }
    //
    //    if (transform.position.x > 75f && transform.position.x < 250f)
    //    {
    //        floorText.text = "2nd Floor";
    //        firstFloorDir.SetActive(false);
    //        secondFloorDir.SetActive(true);
    //        thirdFloorDir.SetActive(false);
    //    }
    //
    //    if (transform.position.x > 250f)
    //    {
    //        floorText.text = "1st Floor";
    //        firstFloorDir.SetActive(true);
    //        secondFloorDir.SetActive(false);
    //        thirdFloorDir.SetActive(false);
    //    }
    //}
    //
    //private void ResetStatus()
    //{
    //    isTeleporting = false;
    //    playerOccupied = false;
    //    isSpotted = false;
    //    canInteract = false;
    //}
    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            //DisplayTextInformation("press 'e' to steal!");
            currentItemBeingInteractedWith = collision.gameObject;
            canInteract = true;
        }
    
        //if(collision.tag == "Directory")
        //{
        //    mallDirectory.SetActive(true);
        //}
        //
        //if (collision.tag == "Transporter" && !canTeleport)
        //{
        //    currentTransporter = collision.GetComponent<Transporter>();
        //    string teleporterInfoText = collision.GetComponent<Transporter>().transporterInfo;
        //    DisplayTextInformation(teleporterInfoText);
        //    canTeleport = true;
        //}
        //
        //if (collision.tag == "Store")
        //{
        //    storeText.text = collision.GetComponent<Store>().storeName;
        //}
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            //CancelTextInformation();
            currentItemBeingInteractedWith = null;
            canInteract = false;
        }
    
        //if (collision.tag == "Directory")
        //{
        //    mallDirectory.SetActive(false);
        //}
        //
        //if (collision.tag == "Transporter")
        //{
        //    currentTransporter = null;
        //    CancelTextInformation();
        //    canTeleport = false;
        //}
    }
    
    //public void DisplayTextInformation(string s)
    //{
    //    textInformation.text = s;
    //    textInformationPanel.SetActive(true);
    //}
    //
    //public void CancelTextInformation()
    //{     
    //    textInformationPanel.SetActive(false);
    //    textInformation.text = "";
    //}
    //
    //public void Kill()
    //{
    //    spr.enabled = false;
    //    deathPanel.SetActive(true);
    //}
}
