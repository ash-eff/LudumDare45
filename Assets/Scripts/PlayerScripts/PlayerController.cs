using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;

namespace Ash.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        public StateMachine<PlayerController> stateMachine;
        public static PlayerController player;

        [SerializeField] private float baseMoveSpeed = 6;
        [SerializeField] private float crawlSpeed = 2;
        [SerializeField] private float dashSpeed = 50;
        public SpriteRenderer playerSprite;

        private bool canMove;
        private float moveSpeed;
        public Animator spriteAnim;
        private Rigidbody2D rb2d;
        private PlayerInputs playerInput;
        public PlayerSurroundings playerSurrounding;

        public bool CanMove { get { return canMove; } set { canMove = value; } }

        private void Awake()
        {
            playerInput = GetComponent<PlayerInputs>();
            rb2d = GetComponent<Rigidbody2D>();
            spriteAnim = GetComponent<Animator>();
            playerSurrounding = GetComponent<PlayerSurroundings>();
            player = this;
            stateMachine = new StateMachine<PlayerController>(this);
            stateMachine.ChangeState(BaseState.Instance);
        }

        private void Update() => stateMachine.Update();
        private void FixedUpdate() => stateMachine.FixedUpdate();

        public void PlayerRun()
        {
            moveSpeed = baseMoveSpeed;
            rb2d.velocity = new Vector2(playerInput.Movement.x, playerInput.Movement.y) * moveSpeed;
        }

        public void StopPlayer()
        {
            rb2d.velocity = Vector3.zero;
        }

        public void PlayerCrawl()
        {
            moveSpeed = crawlSpeed;
            rb2d.velocity = new Vector2(playerInput.Movement.x, playerInput.Movement.y) * moveSpeed;
        }

        public void SetSpriteAnimation()
        {
            Vector3 movement = playerInput.Movement;
            if (movement.x != 0)
            {
                spriteAnim.SetBool("Moving", true);
                spriteAnim.SetBool("RunRightLeft", true);
                spriteAnim.SetBool("RunUp", false);
                spriteAnim.SetBool("RunDown", false);
            }
            else if (movement.y > 0)
            {
                spriteAnim.SetBool("Moving", true);
                spriteAnim.SetBool("RunUp", true);
                spriteAnim.SetBool("RunRightLeft", false);
                spriteAnim.SetBool("RunDown", false);
            }
            else if (movement.y < 0)
            {
                spriteAnim.SetBool("Moving", true);
                spriteAnim.SetBool("RunDown", true);
                spriteAnim.SetBool("RunRightLeft", false);
                spriteAnim.SetBool("RunUp", false);
            }
            else
            {
                spriteAnim.SetBool("Moving", false);
                spriteAnim.SetBool("RunRightLeft", false);
                spriteAnim.SetBool("RunUp", false);
                spriteAnim.SetBool("RunDown", false);
            }
        }

        public void SetSpriteDirection()
        {
            if(playerInput.Movement.x != 0)
            {
                playerSprite.transform.localScale = new Vector2(playerInput.Movement.x, 1f);
            }
        }

        public void SetPlayerSpriteActive(bool isActive)
        {
            playerSprite.enabled = isActive;
        }
    }
}