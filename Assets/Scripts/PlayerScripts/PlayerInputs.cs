using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class PlayerInputs : MonoBehaviour
{
    private Vector3 movement;
    private Vector2 direction;
    private PlayerController playerController;
    public Vector3 Movement { get { return movement; } }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerController.stateMachine.ChangeState(BaseState.Instance);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerController.stateMachine.ChangeState(VentState.Instance);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerController.stateMachine.ChangeState(WaitState.Instance);
        }
    }

    private void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
