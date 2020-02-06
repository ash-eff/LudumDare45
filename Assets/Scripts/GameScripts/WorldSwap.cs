using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwap : MonoBehaviour
{
    public GameObject overworld;
    public GameObject underworld;
    public bool swap;

    public void SwapWorlds()
    {
        if (swap)
        {
            overworld.SetActive(false);
            underworld.SetActive(true);
        }
        else
        {
            overworld.SetActive(true);
            underworld.SetActive(false);
        }
    }
}
