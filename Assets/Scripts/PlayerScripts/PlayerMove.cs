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

    public Vector2 Movement { get { return movement; } set { movement = value; } }
    public Vector2 Direction { set { direction = value; } }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        rb2d = GetComponent<Rigidbody2D>();
        ps = dashObject.GetComponent<ParticleSystem>();
        psEmission = ps.emission;
        psTextureSheet =ps.textureSheetAnimation;
    }

    public void MovePlayer()
    {
        rb2d.velocity = new Vector2(movement.x, movement.y) * playerManager.MoveSpeed;
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
        playerManager.IsDashing = true;
        playerManager.CanDash = false;
        //dashTimerIndicator.fillAmount = 0;
        float dashTimer = playerManager.DashTime;
        while (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            yield return null;
        }


        psEmission.enabled = false;
        playerManager.IsDashing = false;
        float cooldowntimer = 0;
        while (cooldowntimer <= 1)
        {
            cooldowntimer += (Time.deltaTime / playerManager.DashDelay);
            //dashTimerIndicator.fillAmount = cooldowntimer;

            yield return null;
        }

        playerManager.CanDash = true;
        dashObject.SetActive(false);
    }
}
