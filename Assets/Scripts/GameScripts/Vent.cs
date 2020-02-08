using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Vent : MonoBehaviour, IInteractable
{
    public GameObject entrance;
    public GameObject exit;
    public GameObject ventOverlay;
    private PlayerManager player;
    private WorldSwap swap;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        swap = FindObjectOfType<WorldSwap>();
    }

    public void Interact()
    {
        if (player.inVent)
        {
            player.inVent = false;
            player.ignoreObstacles = false;
            player.playerSprite.GetComponent<SpriteRenderer>().enabled = true;
            player.ventLight.gameObject.SetActive(false);
            swap.swap = false;
            swap.SwapWorld();
            ventOverlay.SetActive(false);
            player.transform.position = exit.transform.position;
        }
        else
        {
            player.inVent = true;
            player.ignoreObstacles = true;
            player.playerSprite.GetComponent<SpriteRenderer>().enabled = false;
            player.ventLight.gameObject.SetActive(true);
            swap.swap = true;
            swap.SwapWorld();
            ventOverlay.SetActive(true);
            player.transform.position = entrance.transform.position;
        }
    }

    public string BeingTouched()
    {
        if (player.inVent)
        {
            return "press e to exit vent";
        }

        return "Press E to enter vent";
    }
}
