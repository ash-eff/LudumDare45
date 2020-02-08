using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Animator anim;
    public bool opening;
    public bool closing;
    public bool locked = true;

    private void Update()
    {
        anim.SetBool("Locked", locked);
        anim.SetBool("Opening", opening);
        anim.SetBool("Closing", closing);
    }

    public void Interact()
    {

    }

    public string BeingTouched()
    {
        if (locked)
        {
            return "Door is Locked";
        }

        return "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!locked && !opening)
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
            if (!locked && opening)
            {
                opening = false;
                closing = true;
            }
        }
    }
}
