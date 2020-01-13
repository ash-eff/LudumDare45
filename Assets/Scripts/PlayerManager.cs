using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public LayerMask wallLayer, doorLayer;
    public float xRayCastLength;
    public float yRayCastLength;
    public float xOffsetFromGround;
    public float yOffsetFromGround;

    public float maxSpeed;
    public float moveSpeed;
    public int lives = 3;
    public GameObject noise;
    public GameObject cursor;
    public float normalCursorRadius;
    public float maxCursorRadius;
    private float actualCursorRadius;
    public float valueStolen;

    //public GameObject mallDirectory;
    public GameObject textInformationPanel;
    public TextMeshProUGUI textInformation;
    public PlayerAudio playerAudio;
    public PlayerItemManager itemManager;
    //public TextMeshProUGUI floorText;
    //public TextMeshProUGUI storeText;
    //public GameObject deathPanel;
    //public GameObject hideActionsPanel;
    public TextMeshProUGUI stolenValueText;
    //public TextMeshProUGUI livesTotalText;

    //public GameObject firstFloorDir;
    //public GameObject secondFloorDir;
    //public GameObject thirdFloorDir;
    //public AudioClip lootGrab;
    //public AudioSource audioSource;
    //public Vector2 checkPoint;

    public bool playerOccupied;
    private bool canMove = true;
    private bool isTeleporting;
    private bool isSpotted;
    private bool isKnocking;
    public bool canInteract;
    private bool canTeleport;
    private bool isTargetable;
    private int totalMoneyStolen;
    private bool isDead;
    public bool wallUp, wallRight, wallDown, wallLeft;
    public bool lockedDoor;

    private Animator anim;
    public Vector3 movement;
    private Vector3 castPosition;
    private GameController gameController;
    private GameObject currentItemBeingInteractedWith;
    private SpriteRenderer spr;
    public LockPad lockPad;
    //private Transporter currentTransporter;
    //private MenuController menuController;

    public bool CanMove { get { return canMove; } }
    public bool IsTeleporting { get { return isTeleporting; } }
    public bool IsSpotted { get { return isSpotted; } }
    public bool IsDead { get { return isDead; } }
    public int TotalMoneyStolen { get { return totalMoneyStolen; } }

    private void Awake()
    {
        Cursor.visible = false;
        //menuController = FindObjectOfType<MenuController>();
        anim = GetComponent<Animator>();
        transform.position = transform.position;
        spr = GetComponent<SpriteRenderer>();
        itemManager = GetComponent<PlayerItemManager>();
        //stolenValueText.text = "Money stolen: $0000";
        //livesTotalText.text = "Lives: " + lives.ToString();
        gameController = FindObjectOfType<GameController>();
        //CheckCurrentFloor();
        //checkPoint = transform.position;
        //StartCoroutine(SetPlayerUntargetable(2f));
        moveSpeed = maxSpeed;
    }

    private void Update()
    {
        if (wallUp && !isKnocking || wallRight && !isKnocking || wallDown && !isKnocking || wallLeft && !isKnocking 
            || itemManager.currentItemBeingInteractedWith != null && !itemManager.playerOccupied)
        {
            DisplayInteractButton();
        }
        else
        {
            CancelTextInformation();
        }


        if (isSpotted || itemManager.playerOccupied  || gameController.IsGameOver)
        {
            return;
        }

        stolenValueText.text = "Value Stolen: " + valueStolen.ToString("00.00");

        
        movement = Vector3.zero;
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);

        
        if(wallUp && movement.y == 1 || wallDown && movement.y == -1 || lockedDoor && movement.y == 1)
        {
            movement.y = 0;
        }

        if(wallRight && movement.x == 1 || wallLeft && movement.x == -1)
        {
            movement.x = 0;
        }

        lockPad.gameObject.SetActive(lockedDoor);

        CheckForButtonPress();
        MouseWheelScroll();
        CursorPos();


        if (canMove)
        {
            MovePlayer(movement.normalized);
        }

        //audioSource.volume = menuController.SFXVolume;
    }

    private void FixedUpdate()
    {
        castPosition = new Vector3(transform.position.x + xOffsetFromGround, transform.position.y + yOffsetFromGround, 0f);
        CheckForWalls(castPosition);
    }

    private void MovePlayer(Vector3 direction)
    {
        transform.position = new Vector3(transform.position.x + (direction.x * moveSpeed * Time.deltaTime),
                                         transform.position.y + (direction.y * moveSpeed * Time.deltaTime), 0f);
    }

    private void CheckForWalls(Vector3 castPosition) //checks for doors too, move this to another function
    {
        Debug.DrawRay(castPosition, Vector3.up * yRayCastLength, Color.red);
        Debug.DrawRay(castPosition, -Vector3.up * yRayCastLength, Color.green);
        Debug.DrawRay(castPosition, Vector3.right * xRayCastLength, Color.blue);
        Debug.DrawRay(castPosition, -Vector3.right * xRayCastLength, Color.yellow);
        if (Physics2D.Raycast(castPosition, Vector3.up, yRayCastLength, wallLayer))
        {
            wallUp = true;
        }
        else
        {
            wallUp = false;
        }

        if (Physics2D.Raycast(castPosition, -Vector3.up, yRayCastLength, wallLayer))
        {
            wallDown = true;
        }
        else
        {
            wallDown = false;
        }

        if (Physics2D.Raycast(castPosition, Vector3.right, yRayCastLength, wallLayer))
        {
            wallRight = true;
        }
        else
        {
            wallRight = false;
        }

        if (Physics2D.Raycast(castPosition, -Vector3.right, yRayCastLength, wallLayer))
        {
            wallLeft = true;
        }
        else
        {
            wallLeft = false;
        }

        RaycastHit2D hit = Physics2D.Raycast(castPosition, Vector3.up, yRayCastLength, doorLayer);

        if (hit)
        {
            lockPad.currentLock = hit.transform.parent.gameObject.GetComponent<Lock>();
            lockedDoor = true;
        }
        else
        {
            lockedDoor = false;
        }
    }

    private void CheckForButtonPress()
    {
        if(wallUp ||wallRight || wallDown || wallLeft)
        {
            if (!isKnocking)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isKnocking = true;
                    StartCoroutine(KnockKnock());
                }
            }
        }

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

        if(itemManager.inventoryItems.Count > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                itemManager.ThrowItem(mousePos);
            }
        }

        if (itemManager.canTakeItem && !itemManager.playerOccupied)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (itemManager.currentItemBeingInteractedWith.alreadyStolen)
                {
                    itemManager.StealItem();
                }
                else
                {
                    StartCoroutine(itemManager.FillStealIndicatorBar());
                }
            }
        }      
    }

    void MouseWheelScroll()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            itemManager.itemIndex++;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            itemManager.itemIndex--;
        }

        if(itemManager.itemIndex > itemManager.inventoryItems.Count - 1)
        {
            itemManager.itemIndex = 0;
        }

        if(itemManager.itemIndex < 0)
        {
            itemManager.itemIndex = itemManager.inventoryItems.Count - 1;
        }
    }

    void CursorPos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.position;
        direction = Vector2.ClampMagnitude(direction, actualCursorRadius);
        cursor.transform.position = (Vector2)transform.position + direction;
    }

    IEnumerator KnockKnock()
    {
        Instantiate(noise, transform.position, Quaternion.identity);
        playerAudio.PlayKnock();
        yield return new WaitForSeconds(1f);
        isKnocking = false;
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
    
    public void DisplayInteractButton()
    {
        textInformationPanel.SetActive(true);
    }
    
    public void CancelTextInformation()
    {     
        textInformationPanel.SetActive(false);
    }

    public void AddValue(float addedValue)
    {
        valueStolen += addedValue;
    }

    public void SubtractValue(float subtractedValue)
    {
        valueStolen -= subtractedValue;
    }
 
    //public void Kill()
    //{
    //    spr.enabled = false;
    //    deathPanel.SetActive(true);
    //}
}
