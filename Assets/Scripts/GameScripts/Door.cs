using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator anim;
    public bool open = false;

    public void OpenDoor()
    {
        if (!open)
        {
            open = true;
            anim.SetBool("Opening", true);
        }
    }
}
