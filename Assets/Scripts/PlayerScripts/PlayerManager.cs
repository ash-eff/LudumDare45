using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> hitObstacles = new List<GameObject>();
    public GameObject currentlyTouching;

    #region Editor Variables
    [Header("LayerMasks")]
    //[SerializeField] public LayerMask doorLayer;
    //[SerializeField] public LayerMask wallLayer;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask obstacleLayer;
    //[SerializeField] public LayerMask robotLayer;
    //[SerializeField] public LayerMask screenLayer;
    [Space(2)]

    [Header("Raycast Values")]
    [SerializeField] private int numberOfRays;
    [SerializeField] private float raysWidth;
    [SerializeField] private float raysHeight;
    [SerializeField] private float xRayCastLength =.4f;
    [SerializeField] private float yRayCastLength = .05f;
    [SerializeField] private float yRayDownExtraLength = .4f;
    [SerializeField] private float yRayOffsetFromGround = -.6f;
    [SerializeField] private float itemRadiusCast = 1;
    [Space(2)]

    [Header("Speed Values")]
    [SerializeField] private float baseMoveSpeed = 6;
    [SerializeField] private float crawlSpeed = 2;
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
    public Light2D ventLight;
    
    [Space(2)]

    [Header("GUI")]
    public Image heldItemSprite;
    public TextMeshProUGUI heldItemValueText;
    public TextMeshProUGUI interactText;
    [Space(2)]
    #endregion

    #region Private Variables
    private float valueStolen;
    private float actualCursorRadius;
    public float moveSpeed;

    private bool canMove;
    public bool inVent;
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
    private bool isStealthed;

    private Vector2[] directionsVertical = { Vector2.up, Vector2.down };
    private Vector2[] directionsHorizontal = { Vector2.right, Vector2.left };
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
        gameController = FindObjectOfType<GameController>();
        playerInventory = GetComponent<PlayerInventory>();
        playerActions = GetComponent<PlayerActions>();
        playerMove = GetComponent<PlayerMove>();
        moveSpeed = baseMoveSpeed;
        currentEnergy = maxEnergy;
        StartCoroutine(AddEnergyEverySecond());
        StartCoroutine(CheckForObjects());
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
        CheckForItems();
        CheckForButtonPress();
        CursorPos();
        UpdateGUI();
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
            if (inVent)
            {
                moveSpeed = crawlSpeed;
                playerMove.Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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

    IEnumerator CheckForObjects()
    {
        while (true)
        {
            hitObstacles = new List<GameObject>();
            hitObstacles = CheckForObjectsUpAndDown();

            if(hitObstacles.Count == 0)
            {
                hitObstacles = CheckForObjectsLeftAndRight();
            }

            if (hitObstacles.Count > 0)
            {
                WhatIsThePlayerTouching(hitObstacles);
            }
            else
            {
                currentlyTouching = null;
                interactText.text = "";
            }
            yield return null;
        }
    }

    private void WhatIsThePlayerTouching(List<GameObject> theList)
    {
        GameObject currentGameObject = theList[0];
        int numberOfTimes = 0;
        if (theList.Count > 1)
        {
            foreach (GameObject obj in theList)
            {
                currentGameObject = obj;
                numberOfTimes = 0;
                for (int i = 0; i < theList.Count; i++)
                {
                    if (currentGameObject == theList[i])
                    {
                        numberOfTimes++;
                    }
                }
                if (numberOfTimes > (theList.Count / 2))
                {
                    break;
                }
            }
        }

        currentlyTouching = currentGameObject;
        if(currentlyTouching.transform.tag == "Wall")
        {
            interactText.text = "Press E to knock";
        }
        else if (currentlyTouching.transform.tag == "Locker")
        {
            interactText.text = "Press E to seach";
        }
        else if (currentlyTouching.transform.tag == "Door")
        {
            interactText.text = "Door is locked";
        }
        else if (currentlyTouching.transform.tag == "Vent")
        {
            interactText.text = "Press E to enter vent";
        }
        else
        {
            interactText.text = "Touching undefined";
        }
    }

    private List<GameObject> CheckForObjectsUpAndDown()
    {
        List<GameObject> hitUpDown = new List<GameObject>();
        float widthByRays = raysWidth / (numberOfRays - 1);
        foreach(Vector2 dir in directionsVertical)
        {
            for(int i = 0; i < numberOfRays; i++)
            {
                Vector2 originPosition = (Vector2)transform.position - new Vector2(raysWidth / 2, yRayOffsetFromGround) + (new Vector2(widthByRays, 0) * i);
                Debug.DrawRay(originPosition,  dir * yRayCastLength, Color.red);
                RaycastHit2D hit = Physics2D.Raycast(originPosition, dir, yRayCastLength, obstacleLayer);
                if (hit)
                {
                    hitUpDown.Add(hit.collider.gameObject);
                }
            }
        }

        return hitUpDown;
    }

    private List<GameObject> CheckForObjectsLeftAndRight()
    {
        List<GameObject> hitLeftRight = new List<GameObject>();
        float heightByRays = raysHeight / (numberOfRays - 1);
        foreach (Vector2 dir in directionsHorizontal)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                Vector2 originPosition = (Vector2)transform.position - new Vector2(0, raysHeight / 2 + yRayOffsetFromGround) + (new Vector2(0, heightByRays) * i);
                Debug.DrawRay(originPosition, dir * xRayCastLength, Color.green);
                RaycastHit2D hit = Physics2D.Raycast(originPosition, dir, xRayCastLength, obstacleLayer);
                if (hit)
                {
                    hitLeftRight.Add(hit.collider.gameObject);
                }
            }
        }

        return hitLeftRight;
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

    private bool HasEnoughEnergy()
    {
        if ((currentEnergy - dashEnergy) > 0)
        {
            return true;
        }

        return false;
    }

    private void AdjustEnergy(int amount)
    {
        if ((currentEnergy + amount) >= maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        else
        {
            currentEnergy += amount;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.transform.gameObject.layer == 8)
        //{
        //    touchingWall = true;
        //}

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.transform.gameObject.tag == "Vent")
        //{
        //    
        //    //WorldSwap swap = FindObjectOfType<WorldSwap>();
        //    //Vent thisVent = null;
        //    //thisVent = collision.gameObject.GetComponent<Vent>();
        //    //if (inVent)
        //    //{
        //    //    inVent = false;
        //    //    spr.enabled = true;
        //    //    ventLight.gameObject.SetActive(false);
        //    //    swap.swap = false;
        //    //    swap.SwapWorlds();
        //    //    transform.position = thisVent.exit.transform.position;
        //    //}
        //    //else
        //    //{
        //    //    inVent = true;
        //    //    spr.enabled = false;
        //    //    ventLight.gameObject.SetActive(true);
        //    //    swap.swap = true;
        //    //    swap.SwapWorlds();
        //    //    transform.position = thisVent.entrance.transform.position;
        //    //}
        //}
    }

    IEnumerator AddEnergyEverySecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            AdjustEnergy(1);
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
