using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItemManager : MonoBehaviour
{
    public Image itemSprite;
    public TextMeshProUGUI itemVal;
    public Sprite defaultSprite;
    public List<Item> inventoryItems = new List<Item>();
    public Item heldItem;
    public int itemIndex = 0;
    public GameObject inventory;
    public Item currentItemBeingInteractedWith;
    public GameObject stealPanel;
    public Image interactTimeFill;
    public TextMeshProUGUI itemName;
    public float stealTime;
    public GameObject inventoryHolder;
    public AudioSource audioSource;
    public LayerMask robotLayer;
    public float noiseRadius;

    private PlayerManager playerManager;
    public bool canTakeItem;
    public bool playerOccupied = false;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (inventoryItems.Count != 0)
        {
            heldItem = inventoryItems[itemIndex];
            itemSprite.sprite = heldItem.itemSprite;
            itemVal.text = "$" + heldItem.itemValue.ToString();
        }
        else
        {
            heldItem = null;
            itemSprite.sprite = defaultSprite;
            itemVal.text = "broke";
        }
    }

    public void ThrowItem(Vector2 toPos)
    {       
        heldItem.startPos = transform.position;
        heldItem.endPos = toPos;
        StartCoroutine(heldItem.ThrowItem());
        GetRidOfItem();
    }

    void GetRidOfItem()
    {
        playerManager.SubtractValue(heldItem.itemValue);
        inventoryItems.Remove(heldItem);
        itemIndex++;
        if (itemIndex > inventoryItems.Count)
        {
            itemIndex = 0;
        }

        if (inventoryItems.Count == 0)
        {
            heldItem = null;
            itemIndex = 0;
        }
    }
    
    public void StealItem()
    {
        playerManager.AddValue(currentItemBeingInteractedWith.itemValue);
        currentItemBeingInteractedWith.transform.parent = inventoryHolder.transform;
        inventoryItems.Add(currentItemBeingInteractedWith);
        currentItemBeingInteractedWith.CollectItem();

        canTakeItem = false;
        currentItemBeingInteractedWith = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Item")
        {
            //playerManager.DisplayTextInformation();
            currentItemBeingInteractedWith = collision.gameObject.GetComponent<Item>();
            currentItemBeingInteractedWith.outline.SetActive(true);
            canTakeItem = true;
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
        if(collision.tag == "Item")
        {
            if(currentItemBeingInteractedWith != null)
            {
                currentItemBeingInteractedWith.outline.SetActive(false);
                currentItemBeingInteractedWith = null;
                canTakeItem = false;
            }
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

    public IEnumerator FillStealIndicatorBar()
    {
        canTakeItem = false;
        playerOccupied = true;
        itemName.text = currentItemBeingInteractedWith.itemName;
        stealPanel.SetActive(true);
        float timeOfInteraction = stealTime;
        interactTimeFill.fillAmount = 0f;
        audioSource.Play();
        while (Input.GetButton("Interact"))
        {
            timeOfInteraction -= Time.deltaTime;
            interactTimeFill.fillAmount += Time.deltaTime / stealTime;

            if (timeOfInteraction <= 0)
            {
                currentItemBeingInteractedWith.alreadyStolen = true;
                StealItem();
                break;
            }
            MakeNoise();
            yield return null;
        }

        audioSource.Stop();

        if(currentItemBeingInteractedWith != null)
        {
            canTakeItem = true;
        }

        playerOccupied = false;      
        stealPanel.SetActive(false);
        interactTimeFill.fillAmount = 0f;
    }

    void MakeNoise()
    {
        RaycastHit2D[] nearbyRobots = Physics2D.CircleCastAll(transform.position, noiseRadius, Vector2.right, 0, robotLayer);
        if (nearbyRobots.Length > 0)
        {
            foreach (RaycastHit2D robot in nearbyRobots)
            {
                //Vector2 directionToRobot = robot.transform.position - transform.position;
                //float robotDistFromNoise = directionToRobot.magnitude;
                //float noiseVolume = CheckForNoiseRedction(directionToRobot);
                //
                //if (robotDistFromNoise < noiseVolume)
                //{
                robot.transform.GetComponent<RobotSenses>().HeardANoise(transform.position);
                //}
            }
        }
        //nearbyRobots = null;
    }
}
