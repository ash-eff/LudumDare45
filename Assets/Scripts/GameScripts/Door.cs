using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Animator anim;
    public SpriteRenderer minimapSprite;
    public Color lockedColor;
    public Color unlockedColor;
    public bool opening;
    public bool closing;
    public bool isLocked;

    public bool IsLocked { get { return isLocked; } set { isLocked = value; SetMinimapColor(); } }

    private void Awake()
    {
        IsLocked = true;
    }

    private void Update()
    {
        anim.SetBool("Locked", isLocked);
        anim.SetBool("Opening", opening);
        anim.SetBool("Closing", closing);
    }

    public void Interact()
    {

    }

    public string BeingTouched()
    {
        if (isLocked)
        {
            return "Door is Locked";
        }

        return "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isLocked && !opening)
            {
                closing = false;
                opening = true;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isLocked && opening)
            {
                opening = false;
                closing = true;
            }
        }
    }

    private void SetMinimapColor()
    {
        if (isLocked)
            minimapSprite.color = lockedColor;
        else
            minimapSprite.color = unlockedColor;
    }
}
