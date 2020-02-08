using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, IInteractable
{
    public GameObject containerOverlay;
    public GameObject entrance;
    public GameObject exit;
    private PlayerManager player;
    private Collider2D coll;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void Interact()
    {
        if (player.isHidden)
        {
            coll.isTrigger = false;
            player.isHidden = false;
            player.playerSprite.GetComponent<SpriteRenderer>().enabled = true;
            containerOverlay.SetActive(false);
            player.transform.position = exit.transform.position;
        }
        else
        {
            coll.isTrigger = true;
            player.isHidden = true;
            player.playerSprite.GetComponent<SpriteRenderer>().enabled = false;
            containerOverlay.SetActive(true);
            player.transform.position = entrance.transform.position;
        }
    }

    public string BeingTouched()
    {
        if (player.isHidden)
        {
            return "press e to get out";
        }
        return "press e to hide";
    }
}
