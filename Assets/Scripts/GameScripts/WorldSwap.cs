using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwap : MonoBehaviour
{
    public GameObject underworld;
    public bool swap;

    public void SwapWorld()
    {
        if (swap)
        {
            underworld.SetActive(true);
        }
        else
        {
            underworld.SetActive(false);
        }
    }
}
