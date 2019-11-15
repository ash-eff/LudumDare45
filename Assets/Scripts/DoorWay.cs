using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWay : MonoBehaviour
{
    public bool isOpen = false;
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {        
            if (other.tag == "Player")
            {
                isOpen = true;
                anim.SetBool("isOpen", true);
            }
            if(other.tag == "Robot")
            {
                isOpen = true;
                anim.SetBool("isOpen", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOpen)
        {
            if (other.tag == "Player")
            {
                isOpen = false;
                anim.SetBool("isOpen", false);
            }
            if (other.tag == "Robot")
            {
                isOpen = false;
                anim.SetBool("isOpen", false);
            }
        }
    }
}
