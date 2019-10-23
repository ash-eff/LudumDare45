using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rWalk : MonoBehaviour
{
    public Vector2 startingPos;
    public int maxDistanceFromStart;
    public int leftRayDist, rightRayDist, forwardRayDist;
    public LayerMask obstacleLayer;

    public bool walltInFront;
    public bool wallToLeft;
    public bool wallToRight;

    RaycastHit2D forwardHit;
    RaycastHit2D leftHit;
    RaycastHit2D rightHit;

    private void Update()
    {
        CheckWalls();
    }

    void CheckWalls()
    {
        Debug.DrawRay(transform.position, transform.right * forwardRayDist, Color.red);
        Debug.DrawRay(transform.position, transform.up * leftRayDist, Color.green);
        Debug.DrawRay(transform.position, -transform.up * rightRayDist, Color.blue);

        forwardHit = Physics2D.Raycast(transform.position, transform.right, forwardRayDist, obstacleLayer);
        leftHit = Physics2D.Raycast(transform.position, transform.up, leftRayDist, obstacleLayer);
        rightHit = Physics2D.Raycast(transform.position, -transform.up, rightRayDist, obstacleLayer);

        if (forwardHit)
        {
            walltInFront = true;
        }
        else
        {
            walltInFront = false;
        }

        if (rightHit)
        {
            wallToRight = true;
        }
        else
        {
            wallToRight = false;
        }

        if (leftHit)
        {
            wallToLeft = true;
        }
        else
        {
            wallToLeft = false;
        }
    }
}
