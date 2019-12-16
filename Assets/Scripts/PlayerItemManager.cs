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
        heldItem.transform.parent = null;
        heldItem.spr.enabled = true;
        heldItem.anim.SetBool("Active", true);
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

    //public void PickUpItem()
    //{
    //    heldItem.spr.enabled = false;
    //    heldItem.anim.SetBool("Active", false);
    //    heldItem.transform.parent = inventory.transform;
    //}
    //
    //public void StealItem()
    //{
    //    heldItem.spr.enabled = false;
    //    heldItem.anim.SetBool("Active", false);
    //    heldItem.transform.parent = inventory.transform;
    //}
}
