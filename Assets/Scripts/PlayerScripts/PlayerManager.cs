using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public LayerMask wallLayer, doorLayer, itemLayer, robotLayer;

    public float xRayCastLength;
    public float yRayCastLength;
    public float yRayDownExtraLength;
    public float yRayOffsetFromGround;
    public float itemRadiusCast;
    public float maxSpeed;
    public float moveSpeed;
    public float dashSpeed;
    public float normalCursorRadius;
    public float maxCursorRadius;
    public float noiseRadius;
    public float stealTime;

    public GameObject noisePrefab;
    public GameObject cursor;
    public GameObject eButtonGUI;
    public GameObject rButtonGUI;
    public LockPad lockPad;
    public PlayerAudio playerAudio;
    public Image heldItemSprite;
    public TextMeshProUGUI heldItemValueText;

    private bool canMove;
    private bool playerOccupied;
    private bool isSpotted = false;
    private bool isKnocking;
    private bool canInteract;
    private bool isTargetable;
    private bool isDead = false;
    public bool touchingWall;
    public bool lockedDoor;

    private Animator anim;

    private Vector3 castPosition = Vector3.zero;
    private GameController gameController;
    public Item currentItemBeingInteractedWith;
    //public Item lastItemInteractedWith;
    public Collider2D[] itemsInRange;
    private SpriteRenderer spr;
    private PlayerInventory playerInventory;
    private PlayerActions playerActions;

    private float valueStolen;
    private float actualCursorRadius;

    public bool CanMove { get { return canMove; } }
    public bool PlayerOccupied { get { return playerOccupied; } set { playerOccupied = value; } }
    public bool IsSpotted { get { return isSpotted; } }
    public bool IsDead { get { return isDead; } }

    private void Awake()
    {
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        transform.position = transform.position;
        spr = GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
        playerInventory = GetComponent<PlayerInventory>();
        playerActions = GetComponent<PlayerActions>();
        moveSpeed = maxSpeed;
    }

    private void Update()
    {
        if (touchingWall && !isKnocking || lockedDoor)
        {
            eButtonGUI.SetActive(true);
        }
        else
        {
            eButtonGUI.SetActive(false);
        }
        //
        //if (isSpotted || playerOccupied || gameController.IsGameOver)
        //{
        //    CancelTextInformation();
        //    return;
        //}

        CheckForBarriers();
        CheckForItems();
        CheckForButtonPress();
        CursorPos();
        UpdateGUI();

        //audioSource.volume = menuController.SFXVolume;
    }

    private void CheckForBarriers()
    {
        castPosition = new Vector3(transform.position.x, transform.position.y + yRayOffsetFromGround, 0f);
        RaycastHit2D hitDoor = Physics2D.Raycast(castPosition, Vector3.up, yRayCastLength, doorLayer);
        RaycastHit2D hitWallUp = Physics2D.Raycast(castPosition, Vector3.up, yRayCastLength, wallLayer);
        RaycastHit2D hitWallDown = Physics2D.Raycast(castPosition, -Vector3.up, (yRayCastLength + yRayDownExtraLength), wallLayer);
        RaycastHit2D hitWallRight = Physics2D.Raycast(castPosition, Vector3.right, xRayCastLength, wallLayer);
        RaycastHit2D hitWallLeft = Physics2D.Raycast(castPosition, -Vector3.right, xRayCastLength, wallLayer);
        Debug.DrawRay(castPosition, Vector3.up * yRayCastLength, Color.red);
        Debug.DrawRay(castPosition, -Vector3.up * (yRayCastLength + yRayDownExtraLength), Color.yellow);
        Debug.DrawRay(castPosition, Vector3.right * xRayCastLength, Color.blue);
        Debug.DrawRay(castPosition, -Vector3.right * xRayCastLength, Color.green);

        if (hitDoor)
        {
            lockPad.currentLock = hitDoor.transform.parent.gameObject.GetComponent<Lock>();
            lockedDoor = true;
        }
        else
        {
            lockedDoor = false;
        }

        if (hitWallUp || hitWallDown || hitWallLeft || hitWallRight)
        {
            touchingWall = true;
        }
        else
        {
            touchingWall = false;
        }
    }

    private void CheckForItems()
    {
        itemsInRange = Physics2D.OverlapCircleAll(transform.position, itemRadiusCast, itemLayer);
        
        if(itemsInRange.Length > 0 && !playerOccupied)
        {
            float startingDistance = (itemsInRange[0].transform.position - transform.position).magnitude;
            Item closestsItem = itemsInRange[0].GetComponent<Item>();

            if (currentItemBeingInteractedWith != null)
            {
                rButtonGUI.SetActive(false);
                currentItemBeingInteractedWith.outline.SetActive(false);
                currentItemBeingInteractedWith.canBePickedUp = false;
                currentItemBeingInteractedWith = null;
            }

            foreach(Collider2D item in itemsInRange)
            {
                float distanceToItem = (item.transform.position - transform.position).magnitude;
                if(distanceToItem < startingDistance)
                {
                    startingDistance = distanceToItem;
                    closestsItem = item.GetComponent<Item>();
                }
            }

            rButtonGUI.SetActive(true);
            currentItemBeingInteractedWith = closestsItem;
            closestsItem.outline.SetActive(true);
            closestsItem.canBePickedUp = true;         
        }
        else
        {
            if(currentItemBeingInteractedWith != null)
            {
                rButtonGUI.SetActive(false);
                currentItemBeingInteractedWith.outline.SetActive(false);
                currentItemBeingInteractedWith.canBePickedUp = false;
                currentItemBeingInteractedWith = null;
            }
        }
    }

    private void CheckForButtonPress()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (touchingWall && !isKnocking)
            {
                isKnocking = true;
                StartCoroutine(KnockKnock());
            }

            else if (lockedDoor && !playerActions.isHacking)
            {
                playerActions.HackLock(true);
            }

            else if(lockedDoor && playerActions.isHacking)
            {
                playerActions.HackLock(false);
            }
        }

        // look ahead
        if (Input.GetKey(KeyCode.LeftShift))
        {
            canMove = false;
            actualCursorRadius = maxCursorRadius;
        }
        else
        {
            canMove = true;
            actualCursorRadius = normalCursorRadius;
        }

        // throw item
        if(playerInventory.InventoryCount > 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButtonDown(0))
            {               
                playerActions.ThrowItem(playerInventory.HeldItem, cursor.transform.position);
                playerInventory.RemoveItemFromInventory(playerInventory.HeldItem);
            }
        }

        // pick up item
        if (currentItemBeingInteractedWith != null && currentItemBeingInteractedWith.canBePickedUp && !playerOccupied)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currentItemBeingInteractedWith.alreadyStolen)
                {
                    Debug.Log("Taking");
                    playerInventory.AddItemToInventory(currentItemBeingInteractedWith);
                    currentItemBeingInteractedWith = null;
                }
                else
                {
                    StartCoroutine(playerActions.StealItem(currentItemBeingInteractedWith));
                    currentItemBeingInteractedWith = null;
                }
            }
        }
    }

    private void CursorPos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position;
        direction = Vector2.ClampMagnitude(direction, actualCursorRadius);
        cursor.transform.position = (Vector2)transform.position + direction;
    }

    private void UpdateGUI()
    {
        if(playerInventory.HeldItem != null)
        {
            heldItemSprite.sprite = playerInventory.HeldItem.spr.sprite;
            heldItemValueText.text = "$" + playerInventory.HeldItem.itemValue.ToString();
        }
        else
        {
            heldItemSprite.sprite = null;
            heldItemValueText.text = "BROKE";
        }

    }

    IEnumerator KnockKnock()
    {
        Instantiate(noisePrefab, transform.position, Quaternion.identity);
        playerAudio.PlayAudio(playerAudio.knock);
        yield return new WaitForSeconds(1f);
        isKnocking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == 8)
        {
            touchingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == 8)
        {
            touchingWall = false;
        }
    }

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
