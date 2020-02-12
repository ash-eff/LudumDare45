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
    private WorldSwap swap;
    State<PlayerController> currentState;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        swap = FindObjectOfType<WorldSwap>();
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
        swap.swap = true;
        swap.SwapWorld();
        ventOverlay.SetActive(true);
    }

    public void PlayerLeaveVent()
    {
        swap.swap = false;
        swap.SwapWorld();
        ventOverlay.SetActive(false);
    }
}
