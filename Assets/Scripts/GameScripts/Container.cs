using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, IInteractable
{
    //public GameObject containerOverlay;
    public GameObject entrance;
    public GameObject exit;
    private Collider2D coll;
    private bool isOccupied;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    public void Interact()
    {

    }

    public string BeingTouched()
    {
        if (isOccupied)
        {
            return "press e to get out";
        }
        return "press e to hide";
    }

    public void HidingEntered()
    {
        isOccupied = true;
        coll.isTrigger = true;
        Debug.Log("Container Overlay");
        //containerOverlay.SetActive(true);
    }

    public void HidingExited()
    {
        isOccupied = false;
        coll.isTrigger = false;
        //containerOverlay.SetActive(false);
    }
}
