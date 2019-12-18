using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite itemSprite;
    public Sprite itemOutline;
    public float itemValue;
    public string itemName;
    public bool alreadyStolen = false;

    public GameObject outline;
    public GameObject downArrow;
    public Animator anim;
    public SpriteRenderer spr;
    public SpriteRenderer outlineSpr;
    public Collider2D coll;

    public Vector3 startPos;
    public Vector3 endPos;

    private void Start()
    {
        outlineSpr.sprite = itemOutline;
        itemName = name;
        spr.sprite = itemSprite;
        coll = GetComponent<Collider2D>();
    }

    public IEnumerator ThrowItem()
    {

        anim.SetBool("Active", true);
        transform.parent = null;
        spr.enabled = true;

        while (transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, 15f * Time.deltaTime);
            yield return null;
        }

        if (alreadyStolen)
        {
            downArrow.SetActive(true);
        }

        coll.enabled = true;
    }

    public void CollectItem()
    {
        outline.SetActive(false);
        spr.enabled = false;
        coll.enabled = false;
        downArrow.SetActive(false);
    }
}
