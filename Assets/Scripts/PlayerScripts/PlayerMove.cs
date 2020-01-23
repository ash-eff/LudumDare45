using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Animator spriteAnim;
    private PlayerManager playerManager;
    private Rigidbody2D rb2d;
    private Vector3 movement;
    private Vector2 direction;
    private Vector2 dashPosition;

    private int idleInt = 2;

    public Vector2 Movement { set { movement = value; } }
    public Vector2 Direction { set { direction = value; } }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer()
    {
        rb2d.velocity = new Vector2(movement.x, movement.y) * playerManager.MoveSpeed;
        spriteAnim.SetFloat("DirX", movement.x);
        spriteAnim.SetFloat("DirY", movement.y);

    }

    public void StopPlayer()
    {
        rb2d.velocity = Vector2.zero;
    }
}
