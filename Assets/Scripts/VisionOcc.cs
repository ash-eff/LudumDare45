using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionOcc : MonoBehaviour
{
    public LayerMask occlusionMask;
    public Animator anim;
    public float rayOffset;
    public float rayLength;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayOffset), Vector2.up, rayLength, occlusionMask);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayOffset), Vector2.up * rayLength, Color.blue);

        anim.SetBool("obscured", hit);
    }
}
