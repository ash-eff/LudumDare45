using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuableItem : MonoBehaviour
{
    //public string itemName;
    //public int minValue, maxValue;
    //[Range(0,1)]
    //public float chanceForGoldenItem;
    //public int itemValue;
    //public string[] itemNames;
    //public bool isGolden;
    //public Sprite goldenSprite;

    public bool isClaimed;
    
    private SpriteRenderer spr;
    private Collider2D col;

    public Vector2 originalItemLocation;
    public Vector2 currentItemLocation;

    private void Start()
    {
        currentItemLocation = transform.position;
        spr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void CheckIfItemIsOutOfPosition()
    {
        if(originalItemLocation != currentItemLocation)
        {
            PickUpItem();
        }
    }

    public void PickUpItem()
    {
        col.enabled = false;
        spr.enabled = false;
        transform.position = originalItemLocation;
    }

    public void PlaceItem()
    {
        col.enabled = true;
        spr.enabled = true;
    }

    public void DropItem(Vector2Int atLocation)
    {
        isClaimed = false;
        transform.position = new Vector2(atLocation.x, atLocation.y);
        col.enabled = true;
        spr.enabled = true;
    }
}
