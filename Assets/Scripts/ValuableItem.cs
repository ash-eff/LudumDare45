using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuableItem : MonoBehaviour
{
    //public enum ItemType { Clothing, General, Sporting, Electronic, Children, Pets, Entertainment, Utility };
    //public ItemType itemType;
    public string itemName;
    public int minValue, maxValue;
    [Range(0,1)]
    public float chanceForGoldenItem;
    public int itemValue;
    public string[] itemNames;
    public bool isGolden;
    public Sprite goldenSprite;

    private SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        itemValue = GetItemValue();
        itemName = GetItemName();
    }

    int GetItemValue()
    {
        if (CheckForGoldenItem())
        {
            spr.sprite = goldenSprite;
            itemValue = maxValue * 10;
            return itemValue;
        }

        itemValue = Random.Range(minValue, maxValue);
        return itemValue;
    }

    bool CheckForGoldenItem()
    {
        float chance = Random.value;
        if(chance <= chanceForGoldenItem)
        {
            isGolden = true;
            return isGolden;
        }

        return false;
    }

    string GetItemName()
    {
        if (isGolden)
        {
            return "Golden Shirt";
        }

        return itemNames[Random.Range(0, itemNames.Length)];
    }
}
