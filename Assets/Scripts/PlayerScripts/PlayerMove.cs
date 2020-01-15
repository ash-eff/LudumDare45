using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerManager playerManager;
    private Rigidbody2D rb2d;
    private Vector3 movement;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {     
        if (!playerManager.PlayerOccupied)
        {
            MovePlayer();
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void MovePlayer()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        rb2d.velocity = new Vector2(movement.x, movement.y) * playerManager.moveSpeed;
    }
}
