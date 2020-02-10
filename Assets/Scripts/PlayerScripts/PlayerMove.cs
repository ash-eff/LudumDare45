using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Animator spriteAnim;
    public GameObject dashObject;
    public Sprite rightRunSprite, leftRunSprite, upRunSprite, downRunSprite;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule psEmission;
    private ParticleSystem.TextureSheetAnimationModule psTextureSheet;
    private PlayerManager playerManager;
    private Rigidbody2D rb2d;
    private Vector3 movement;
    private Vector2 direction;
    private Vector2 dashPosition;
    private bool isDashing;
    private bool canMove;
    private bool canDash = true;
    public Vector2 Movement { get { return movement; } set { movement = value; } }
    public Vector2 Direction { set { direction = value; } }

    [Header("Speed Values")]
    [SerializeField] private float baseMoveSpeed = 6;
    [SerializeField] private float crawlSpeed = 2;
    [SerializeField] private float dashSpeed = 50;
    [SerializeField] private float dashTime = .25f;
    [SerializeField] private float dashDelay = 1;
    public float moveSpeed;
    private Animator anim;
    public GameObject playerSprite;

    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float DashDelay { get { return dashDelay; } set { dashDelay = value; } }
    public float DashTime { get { return dashTime; } set { dashTime = value; } }
    public bool IsDashing { get { return isDashing; } set { isDashing = value; } }
    public bool CanDash { get { return canDash; } set { canDash = value; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        rb2d = GetComponent<Rigidbody2D>();
        ps = dashObject.GetComponent<ParticleSystem>();
        psEmission = ps.emission;
        psTextureSheet =ps.textureSheetAnimation;
        moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        if (movement.x == 0 && movement.y == 0)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
        }

        if (movement.x != 0 && !isDashing)
        {
            playerSprite.transform.localScale = new Vector2(movement.x, 1f);
        }
    }

    private void FixedUpdate()
    {
        //if (isDashing)
        //{
        //    moveSpeed = dashSpeed;
        //    Movement = new Vector2(dashDirection.x, dashDirection.y);
        //}
        //if (inVent)
        //{
        //    moveSpeed = crawlSpeed;
        //    Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //}
        //else
        //{
        //    moveSpeed = baseMoveSpeed;
        //    Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //}

        MovePlayer();
    }

    public void MovePlayer()
    {
        rb2d.velocity = new Vector2(movement.x, movement.y) * moveSpeed;
        if(movement.x != 0)
        {
            spriteAnim.SetBool("RunRightLeft", true);
            spriteAnim.SetBool("RunUp", false);
            spriteAnim.SetBool("RunDown", false);
        }
        else if(movement.y > 0)
        {
            spriteAnim.SetBool("RunUp", true);
            spriteAnim.SetBool("RunRightLeft", false);
            spriteAnim.SetBool("RunDown", false);
        }
        else if(movement.y < 0)
        {
            spriteAnim.SetBool("RunDown", true);
            spriteAnim.SetBool("RunRightLeft", false);
            spriteAnim.SetBool("RunUp", false);
        }
        else
        {
            spriteAnim.SetBool("RunRightLeft", false);
            spriteAnim.SetBool("RunUp", false);
            spriteAnim.SetBool("RunDown", false);
        }
    }

    public void StopPlayer()
    {
        rb2d.velocity = Vector2.zero;
    }

    public void Dash(Vector2 dir)
    {
        direction = dir;
        float angleInDegrees = Vector2.SignedAngle(transform.position, direction);
        SwapParticleSprites((angleInDegrees + 180));
        StartCoroutine(DashTimer());
    }

    void SwapParticleSprites(float angle)
    {
        psTextureSheet.RemoveSprite(0);
        if (angle < 22 && angle > 0 || angle > 338)
        {
            psTextureSheet.AddSprite(upRunSprite);
        }
        else if (angle > 158 && angle < 202)
        {
            psTextureSheet.AddSprite(downRunSprite);
        }
        else if(angle > 202 && angle < 338)
        {
            psTextureSheet.AddSprite(rightRunSprite);
        }
        else if(angle > 22 && angle < 158)
        {
            psTextureSheet.AddSprite(leftRunSprite);
        }
        else
        {
            psTextureSheet.AddSprite(rightRunSprite);
        }
    }

    IEnumerator DashTimer()
    {

        //psTextureSheet.
        psEmission.enabled = true;
        dashObject.SetActive(true);
        isDashing = true;
        canDash = false;
        //dashTimerIndicator.fillAmount = 0;
        float dashTimer = dashTime;
        while (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            yield return null;
        }


        psEmission.enabled = false;
        isDashing = false;
        float cooldowntimer = 0;
        while (cooldowntimer <= 1)
        {
            cooldowntimer += (Time.deltaTime / dashDelay);
            //dashTimerIndicator.fillAmount = cooldowntimer;

            yield return null;
        }

        canDash = true;
        dashObject.SetActive(false);
    }
}
