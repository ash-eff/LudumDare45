using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("KnockKnock");
    }

    public string BeingTouched()
    {
        return "press e to knock";
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
