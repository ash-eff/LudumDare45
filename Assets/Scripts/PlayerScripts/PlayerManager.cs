using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    #region Editor Variables
    [Header("LayerMasks")]
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public LayerMask doorLayer;
    [SerializeField] public LayerMask itemLayer;
    [SerializeField] public LayerMask robotLayer;
    [Space(2)]

    [Header("Raycast Values")]
    [SerializeField] private float xRayCastLength =.4f;
    [SerializeField] private float yRayCastLength = .05f;
    [SerializeField] private float yRayDownExtraLength = .4f;
    [SerializeField] private float yRayOffsetFromGround = -.6f;
    [SerializeField] private float itemRadiusCast = 1;
    [Space(2)]

    [Header("Speed Values")]
    [SerializeField] private float baseMoveSpeed = 6;
    [SerializeField] private float dashSpeed = 50;
    [Space(2)]

    [Header("Time Values")]
    [SerializeField] private float dashTime = .25f;
    [SerializeField] private float dashDelay = 1;
    [SerializeField] private float stealTime = 1;
    [Space(2)]

    [Header("Energy Values")]
    [SerializeField] private int dashEnergy = 23;
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int currentEnergy = 0;
    [Space(2)]

    [Header("Cursor Values")]
    [SerializeField] private float normalCursorRadius = 7.5f;
    [SerializeField] private float maxCursorRadius = 9;
    [Space(2)]

    [Header("Components")]
    public GameObject tr;
    public GameObject cursor;
    public LockPad lockPad;
    public PlayerAudio playerAudio;
    [Space(2)]

    [Header("GUI")]
    public GameObject eButtonGUI;
    public GameObject rButtonGUI;
    public Image heldItemSprite;
    public TextMeshProUGUI heldItemValueText;
    [Space(2)]
    #endregion

    #region Private Variables
    private float valueStolen;
    private float actualCursorRadius;
    private float moveSpeed;

    private bool canMove;
    private bool isDashing;
    private bool playerOccupied;
    private bool isSpotted = false;
    private bool isKnocking;
    private bool canInteract;
    private bool isTargetable;
    private bool isDead = false;
    private bool canDash = true;
    private bool touchingWall;
    private bool lockedDoor;

    private Vector2 dashDirection;
    private Vector3 castPosition = Vector3.zero;
    private GameController gameController;
    private Item currentItemBeingInteractedWith;
    private Collider2D[] itemsInRange;
    private SpriteRenderer spr;
    private Animator anim;
    private PlayerInventory playerInventory;
    private PlayerActions playerActions;
    private PlayerMove playerMove;

    public float MoveSpeed { get { return moveSpeed; } }
    public float StealTime { get { return stealTime; } }
    public bool CanMove { get { return canMove; } }
    public bool PlayerOccupied { get { return playerOccupied; } set { playerOccupied = value; } }
    public bool IsSpotted { get { return isSpotted; } }
    public bool IsDead { get { return isDead; } }
    public bool IsKnocking { set { isKnocking = value; } }
    #endregion

    private void Awake()
    {
        Cursor.visible = false;
        anim = GetComponent<Animator>();
        transform.position = transform.position;
        spr = GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
        playerInventory = GetComponent<PlayerInventory>();
        playerActions = GetComponent<PlayerActions>();
        playerMove = GetComponent<PlayerMove>();
        moveSpeed = baseMoveSpeed;
        currentEnergy = maxEnergy;
        StartCoroutine(AddEnergyEverySecond());
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

        if(!playerOccupied)
        {
            if (isDashing)
            {
                moveSpeed = dashSpeed;
                playerMove.Movement = new Vector2(dashDirection.x, dashDirection.y);
            }
            else
            {
                moveSpeed = baseMoveSpeed;
                playerMove.Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));              
            }

            playerMove.MovePlayer();
        }
        else
        {
            playerMove.Movement = Vector3.zero;
        }

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
        // knock and hack
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (touchingWall && !isKnocking)
            {
                isKnocking = true;
                StartCoroutine(playerActions.Knock());
            }

            else if (lockedDoor && !playerActions.isHacking)
            {
                StartCoroutine(playerActions.HackLocks(true));
            }

            else if(lockedDoor && playerActions.isHacking)
            {
                StartCoroutine(playerActions.HackLocks(false));
            }
        }

        // dash
        if ((Input.GetKeyDown(KeyCode.Space)))
        {
            if (!isDashing && canDash)
            {
                if((currentEnergy - dashEnergy) > 0)
                {
                    AdjustEnergy(-dashEnergy);
                    dashDirection = (cursor.transform.position - transform.position).normalized;
                    StartCoroutine(DashTimer());
                }
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

    IEnumerator DashTimer()
    {
        ParticleSystem.EmissionModule ps = tr.GetComponent<ParticleSystem>().emission;
        ps.enabled = true;
        tr.SetActive(true);
        isDashing = true;
        canDash = false;
        float dashTimer = dashTime;
        while (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            yield return null;
        }

        
        ps.enabled = false;
        isDashing = false;
        yield return new WaitForSeconds(dashDelay);
        canDash = true;
        tr.SetActive(false);

    }

    IEnumerator AddEnergyEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            AdjustEnergy(1);
        }
    }

    void AdjustEnergy(int amount)
    {
        if((currentEnergy + amount) >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        else
        {
            currentEnergy += amount;
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
