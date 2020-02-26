using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class Vent : MonoBehaviour, IInteractable
{
    public GameObject entrance;
    public GameObject exit;
    public GameObject ventOverlay;
    private PlayerController player;
    public Room thisRoom;
    State<PlayerController> currentState;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        thisRoom = GetComponentInParent<Room>();
    }

    public void Interact()
    {

    }

    public string BeingTouched()
    {
        if (player.stateMachine.currentState == VentState.Instance)
        {
            return "press e to exit vent";
        }

        return "Press E to enter vent";
    }

    public void PlayerEnterVent()
    {
        thisRoom.SwapToVents(true);
    }

    public void PlayerLeaveVent()
    {
        thisRoom.SwapToVents(false);
    }
}
