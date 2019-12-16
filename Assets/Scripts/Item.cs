using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite itemSprite;
    public float itemValue;

    public Animator anim;
    public SpriteRenderer spr;

    public Vector3 startPos;
    public Vector3 endPos;

    private void Start()
    {
        spr.sprite = itemSprite;
    }

    public IEnumerator ThrowItem()
    {
        while(transform.position != endPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, 15f * Time.deltaTime);
            yield return null;
        }
    }
}
