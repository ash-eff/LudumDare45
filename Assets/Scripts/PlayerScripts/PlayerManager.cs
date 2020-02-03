using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public List<RaycastResult> hitObjects = new List<RaycastResult>();

    #region Editor Variables
    [Header("LayerMasks")]
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public LayerMask doorLayer;
    [SerializeField] public LayerMask itemLayer;
    [SerializeField] public LayerMask robotLayer;
    [SerializeField] public LayerMask screenLayer;
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
    [SerializeField] private int dashEnergy = 5;
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int currentEnergy = 0;
    [Space(2)]

    [Header("Cursor Values")]
    [SerializeField] private float normalCursorRadius = 7.5f;
    [SerializeField] private float maxCursorRadius = 9;
    [Space(2)]

    [Header("Color Values")]
    [SerializeField] private Color startingColor;
    [SerializeField] private Color stealthedColor;
    [Space(2)]

    [Header("Components")]

    public GameObject cursor;
    public Sprite targetCursor, pointerCursor;
    public Lock currentLock;
    public PlayerAudio playerAudio;
    public Animator soundIconAnim;
    public Animator dashIconAnim;
    public Animator grabIconAnim;
    public Animator hackIconAnim;
    public HackingTest hackController;
    public Image dashTimerIndicator;
    public Image energyFillIndicator;
    public AudioSource song;
    public AudioSource bit;
    [Space(2)]

    [Header("GUI")]
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
    private bool itemInRange;
    private bool playerOccupied;
    private bool isTerminalOpen;
    private bool isSpotted = false;
    private bool isKnocking;
    private bool canInteract;
    private bool isTargetable;
    private bool isDead = false;
    private bool canDash = true;
    private bool touchingWall;
    private bool touchingLockedDoor;
    private bool isStealthed;

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

    public Transform terminalObjectClicked;

    public float MoveSpeed { get { return moveSpeed; } }
    public float StealTime { get { return stealTime; } }
    public float DashDelay { get { return dashDelay; } set { dashDelay = value; } }
    public float DashTime { get { return dashTime; } set { dashTime = value; } }
    public bool IsDashing { get { return isDashing; } set { isDashing = value; } }
    public bool CanDash { get { return canDash; } set { canDash = value; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
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
        spr.color = startingColor;
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
        if (isTerminalOpen)
        {
            cursor.GetComponent<SpriteRenderer>().sprite = pointerCursor;
            song.volume = 0;
            bit.volume = 1;
        }
        else
        {
            cursor.GetComponent<SpriteRenderer>().sprite = targetCursor;
            song.volume = .5f;
            bit.volume = 0;
        }
        if (isSpotted || playerOccupied) //|| gameController.IsGameOver)
        {
            playerMove.Movement = Vector3.zero;
            //return;
        }

        anim.SetBool("Hacking", isTerminalOpen);
        
        if (playerMove.Movement.x == 0 && playerMove.Movement.y == 0)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }

        if(playerMove.Movement.x != 0 && !isDashing)
        {
            transform.localScale = new Vector2(playerMove.Movement.x, 1f);
        }

        energyFillIndicator.fillAmount = ((float)currentEnergy / (float)maxEnergy);
        SetIconAnimations();
        CheckForBarriers();
        CheckForItems();
        CheckForButtonPress();
        CursorPos();
        UpdateGUI();

        //audioSource.volume = menuController.SFXVolume;
    }

    private void FixedUpdate()
    {
        if (!playerOccupied)
        {
            if (isDashing)
            {
                moveSpeed = dashSpeed;
                playerMove.Movement = new Vector2(dashDirection.x, dashDirection.y);
            }
            else
            {
                moveSpeed = baseMoveSpeed;
                playerMove.Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }

            playerMove.MovePlayer();

        }
        else
        {
            playerMove.StopPlayer();
        }
    }

    private void SetIconAnimations()
    {
        if (touchingWall && !isKnocking)
        {
            soundIconAnim.SetBool("Active", true);
        }
        else
        {
            soundIconAnim.SetBool("Active", false);
        }

        if (touchingLockedDoor)
        {
            hackIconAnim.SetBool("Active", true);
        }
        else
        {
            hackIconAnim.SetBool("Active", false);
        }

        if (canDash && HasEnoughEnergy())
        {
            dashIconAnim.SetBool("Active", true);
        }
        else
        {
            dashIconAnim.SetBool("Active", false);
        }

        if (currentItemBeingInteractedWith != null)
        {
            grabIconAnim.SetBool("Active", true);
        }
        else
        {
            grabIconAnim.SetBool("Active", false);
        }
    }

    private void CheckForBarriers()
    {
        castPosition = new Vector3(transform.position.x, transform.position.y + yRayOffsetFromGround, 0f);
        RaycastHit2D hitDoor = Physics2D.Raycast(castPosition, Vector3.up, yRayCastLength, doorLayer);
        RaycastHit2D hitWallUp = Physics2D.Raycast(castPosition, Vector3.up, yRayCastLength, wallLayer);
        RaycastHit2D hitWallDown = Physics2D.Raycast(castPosition, -Vector3.up, (yRayCastLength + yRayDownExtraLength), wallLayer);
        RaycastHit2D hitWallRightLeft = Physics2D.Raycast(castPosition, Vector2.right * transform.localScale.x, xRayCastLength, wallLayer);

        Debug.DrawRay(castPosition, Vector3.up * yRayCastLength, Color.red);
        Debug.DrawRay(castPosition, -Vector3.up * (yRayCastLength + yRayDownExtraLength), Color.yellow);
        Debug.DrawRay(castPosition, (Vector2.right * transform.localScale.x) * xRayCastLength, Color.blue);

        if (hitDoor)
        {
            touchingLockedDoor = true;
            //hackController.hackableSource = hitDoor.collider.GetComponentInParent<HackableSource>();
        }
        else
        {
            touchingLockedDoor = false;
            //currentLock = null;
        }

        anim.SetBool("StealthedUp", hitWallUp);
        anim.SetBool("StealthedDown", hitWallDown);
        anim.SetBool("StealthedRightLeft", hitWallRightLeft);


        if (hitWallUp && !isStealthed)
        {
            isStealthed = true;
            spr.sortingOrder = 3;
            StartCoroutine(StealthTransition(startingColor, stealthedColor));
        }

        if (hitWallDown && !isStealthed)
        {
            isStealthed = true;
            spr.sortingOrder = 5;
            //StartCoroutine(StealthTransition(new Color(stealthedColor.r, stealthedColor.g, stealthedColor.b, startingColor.a), stealthedColor));
        }

        if (hitWallRightLeft && !isStealthed)
        {
            isStealthed = true;
            spr.sortingOrder = 3;
            StartCoroutine(StealthTransition(startingColor, stealthedColor));
        }

        if (!hitWallUp && !hitWallDown && !hitWallRightLeft && isStealthed)
        {
            isStealthed = false;
            spr.sortingOrder = 3;
            StartCoroutine(StealthTransition(stealthedColor, startingColor));
        }

        if (hitWallUp || hitWallDown || hitWallRightLeft)
        {
            touchingWall = true;
        }
        else
        {
            touchingWall = false;
        }
    }

    IEnumerator StealthTransition(Color fromColor, Color toColor)
    {
        float lerptime = .25f;
        float currentLerptime = 0;
        
        while (currentLerptime < lerptime)
        {
            currentLerptime += Time.deltaTime;
            float perc = currentLerptime / lerptime;
            spr.color = Color.Lerp(fromColor, toColor, perc);
            yield return null;
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
                currentItemBeingInteractedWith.outline.SetActive(false);
                currentItemBeingInteractedWith.canBePickedUp = false;
                currentItemBeingInteractedWith = null;
                itemInRange = false;
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

            currentItemBeingInteractedWith = closestsItem;
            closestsItem.outline.SetActive(true);
            closestsItem.canBePickedUp = true;
            itemInRange = true;
        }
        else
        {
            if(currentItemBeingInteractedWith != null)
            {
                currentItemBeingInteractedWith.outline.SetActive(false);
                currentItemBeingInteractedWith.canBePickedUp = false;
                currentItemBeingInteractedWith = null;
                itemInRange = false;
            }
        }
    }

    private void CheckForButtonPress()
    {
        // knock
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (touchingWall && !isKnocking)
            {
                isKnocking = true;
                StartCoroutine(playerActions.Knock());
            }
        }

        // hack
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {         
            if (!isTerminalOpen)
            {
                isTerminalOpen = true;
                hackController.OpenTerminal();
                //StartCoroutine(playerActions.HackLocks());
            }
            else
            {
                isTerminalOpen = false;
                hackController.CloseTerminal();
                //PlayerOccupied = false;
            }
        }

        if (isTerminalOpen)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                isTerminalOpen = false;
                hackController.CloseTerminal();
            }
        }

        // dash
        if ((Input.GetKeyDown(KeyCode.Alpha2)))
        {
            if (!isDashing && canDash)
            {
                if(HasEnoughEnergy())
                {
                    AdjustEnergy(-dashEnergy);
                    dashDirection = (cursor.transform.position - transform.position).normalized;
                    playerMove.Dash(cursor.transform.position - transform.position);
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
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("Pressing 3");
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

        if (!isTerminalOpen)
        {
            direction = Vector2.ClampMagnitude(direction, actualCursorRadius);
        }
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

        if(collision.transform.tag == "Hackable")
        {
            hackController.hackableSource = null;
            hackController.isConnectedToHost = false;
            hackController.hackableSource = collision.transform.GetComponentInParent<HackableSource>();
            hackController.HostAvailable();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == 8)
        {
            touchingWall = false;
        }
    }

    private bool HasEnoughEnergy()
    {
        if((currentEnergy - dashEnergy) > 0)
        {
            return true;
        }

        return false;
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
