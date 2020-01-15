using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public GameObject inventoryHolder;

    private List<Item> inventoryItems = new List<Item>();

    private Item heldItem = null;
    private int inventoryIndex = 0;
    private int inventoryCount = 0;

    //private Image itemSprite;
    //private TextMeshProUGUI itemVal;
    //private Sprite defaultSprite;

    //private GameObject stealPanel;
    //private Image interactTimeFill;
    //private TextMeshProUGUI itemName;
    //private float stealTime;
    //private GameObject inventoryHolder;
    //private LayerMask robotLayer;
    //private float noiseRadius;

    public Item HeldItem { get { return heldItem; } }
    public int InventoryIndex { get { return inventoryIndex; } }
    public int InventoryCount { get { return inventoryCount; } }

    void Update()
    {
        CheckInventory();
        MouseWheelScroll();
    }

    void CheckInventory()
    {
        inventoryCount = inventoryItems.Count;

        if (inventoryItems.Count != 0)
        {
            heldItem = inventoryItems[inventoryIndex];
        }
        else
        {
            heldItem = null;
        }
    }

    void MouseWheelScroll()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
           inventoryIndex++;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            inventoryIndex--;
        }

        if (inventoryIndex > inventoryCount - 1)
        {
            inventoryIndex = 0;
        }

        if (inventoryIndex < 0)
        {
            inventoryIndex = inventoryCount - 1;
        }
    }

    public void AddItemToInventory(Item itemToAdd)
    {
        inventoryItems.Add(itemToAdd);
        itemToAdd.transform.parent = inventoryHolder.transform;
        itemToAdd.CollectItem();
    }

    public void RemoveItemFromInventory(Item itemToRemove)
    {
        inventoryItems.Remove(itemToRemove);
        inventoryIndex++;
        if (inventoryIndex > inventoryItems.Count)
        {
            inventoryIndex = 0;
        }
        
        if (inventoryItems.Count == 0)
        {
            heldItem = null;
            inventoryIndex = 0;
        }
    } 
    

}
