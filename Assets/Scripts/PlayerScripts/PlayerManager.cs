using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal;
using Ash.PlayerController;

public class PlayerCon : MonoBehaviour
{
    //[Header("Cursor Values")]
    //[SerializeField] private float normalCursorRadius = 7.5f;
    //[SerializeField] private float maxCursorRadius = 9;
    //[Space(2)]
    //
    //[Header("Components")]
    //public GameObject cursor;
    //
    //public Sprite targetCursor, pointerCursor;
    //
    //public PlayerAudio playerAudio;
    //
    //public HackingTest hackController;
    //
    //public AudioSource song;
    //public AudioSource bit;
    //public Light2D ventLight;
    //
    //#region Private Variables
    //private float valueStolen;
    //private float actualCursorRadius;
    //
    public bool ignoreObstacles;
    public bool isHidden;
    public bool isHacking;
    public bool inVent;
    private bool itemInRange;
    private bool playerOccupied;
    private bool isTerminalOpen;
    private bool isSpotted = false;
    private bool isKnocking;
    private bool canInteract;
    private bool isTargetable;
    private bool isDead = false;
    //
    //private bool touchingWall;
    //private bool isStealthed;
    //
    //private Vector2 dashDirection;
    //private Vector3 castPosition = Vector3.zero;
    //private GameController gameController;
    //private Item currentItemBeingInteractedWith;
    //private Collider2D[] itemsInRange;
    //private SpriteRenderer spr;
    //
    //private PlayerInventory playerInventory;
    //private PlayerActions playerActions;
    //
    //public Transform terminalObjectClicked;
    //
    //public bool PlayerOccupied { get { return playerOccupied; } set { playerOccupied = value; } }
    //public bool IsSpotted { get { return isSpotted; } }
    //public bool IsDead { get { return isDead; } }
    //public bool IsKnocking { set { isKnocking = value; } }
    //#endregion

    PlayerController playerController;
    PlayerSurroundings playerSurroundings;


    private void Awake()
    {
        playerSurroundings = GetComponent<PlayerSurroundings>();
        playerController = GetComponent<PlayerController>();

        //
        //transform.position = transform.position;
        //spr = GetComponent<SpriteRenderer>();
        //gameController = FindObjectOfType<GameController>();
        //playerInventory = GetComponent<PlayerInventory>();
        //playerActions = GetComponent<PlayerActions>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //playerSurroundings.stateMachine.ChangeState(TestState2.Instance);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //playerSurroundings.stateMachine.ChangeState(TestState.Instance);
        }
        //if (isTerminalOpen)
        //{
        //    cursor.GetComponent<SpriteRenderer>().sprite = pointerCursor;
        //    song.volume = 0;
        //    bit.volume = 1;
        //}
        //else
        //{
        //    cursor.GetComponent<SpriteRenderer>().sprite = targetCursor;
        //    song.volume = .5f;
        //    bit.volume = 0;
        //}
        ////if (isSpotted || playerOccupied)
        ////{
        ////    playerMove.Movement = Vector3.zero;
        ////    //return;
        ////}
        //
        //CursorPos();
        //
        //
        //if (ignoreObstacles)
        //{
        //    Physics2D.IgnoreLayerCollision(9, 8, true);
        //}
        //else
        //{
        //    Physics2D.IgnoreLayerCollision(9, 8, false);
        //}
    }



    //private void CheckForItems()
    //{
    //    itemsInRange = Physics2D.OverlapCircleAll(transform.position, itemRadiusCast, itemLayer);
    //    
    //    if(itemsInRange.Length > 0 && !playerOccupied)
    //    {
    //        float startingDistance = (itemsInRange[0].transform.position - transform.position).magnitude;
    //        Item closestsItem = itemsInRange[0].GetComponent<Item>();
    //
    //        if (currentItemBeingInteractedWith != null)
    //        {
    //            currentItemBeingInteractedWith.outline.SetActive(false);
    //            currentItemBeingInteractedWith.canBePickedUp = false;
    //            currentItemBeingInteractedWith = null;
    //            itemInRange = false;
    //        }
    //
    //        foreach(Collider2D item in itemsInRange)
    //        {
    //            float distanceToItem = (item.transform.position - transform.position).magnitude;
    //            if(distanceToItem < startingDistance)
    //            {
    //                startingDistance = distanceToItem;
    //                closestsItem = item.GetComponent<Item>();
    //            }
    //        }
    //
    //        currentItemBeingInteractedWith = closestsItem;
    //        closestsItem.outline.SetActive(true);
    //        closestsItem.canBePickedUp = true;
    //        itemInRange = true;
    //    }
    //    else
    //    {
    //        if(currentItemBeingInteractedWith != null)
    //        {
    //            currentItemBeingInteractedWith.outline.SetActive(false);
    //            currentItemBeingInteractedWith.canBePickedUp = false;
    //            currentItemBeingInteractedWith = null;
    //            itemInRange = false;
    //        }
    //    }
    //}

    //private void CursorPos()
    //{
    //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 direction = mousePos - (Vector2)transform.position;
    //
    //    if (!isTerminalOpen)
    //    {
    //        direction = Vector2.ClampMagnitude(direction, actualCursorRadius);
    //    }
    //    cursor.transform.position = (Vector2)transform.position + direction;
    //}

    //private void UpdateGUI()
    //{
    //    if(playerInventory.HeldItem != null)
    //    {
    //        heldItemSprite.sprite = playerInventory.HeldItem.spr.sprite;
    //        heldItemValueText.text = "$" + playerInventory.HeldItem.itemValue.ToString();
    //    }
    //    else
    //    {
    //        heldItemSprite.sprite = null;
    //        heldItemValueText.text = "BROKE";
    //    }
    //
    //}

    //private bool HasEnoughEnergy()
    //{
    //    if ((currentEnergy - dashEnergy) > 0)
    //    {
    //        return true;
    //    }
    //
    //    return false;
    //}

    //private void AdjustEnergy(int amount)
    //{
    //    if ((currentEnergy + amount) >= maxEnergy)
    //    {
    //        currentEnergy = maxEnergy;
    //    }
    //    else
    //    {
    //        currentEnergy += amount;
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.transform.tag == "Hackable")
    //    {
    //        hackController.hackableSource = null;
    //        hackController.isConnectedToHost = false;
    //        hackController.hackableSource = collision.transform.GetComponentInParent<HackableSource>();
    //        hackController.HostAvailable();
    //    }
    //}
    //
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.transform.gameObject.layer == 8)
    //    {
    //        touchingWall = false;
    //    }
    //}

    //IEnumerator AddEnergyEverySecond()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1f);
    //        AdjustEnergy(1);
    //    }
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
}
