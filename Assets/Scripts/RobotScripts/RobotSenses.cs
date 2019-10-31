using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSenses : MonoBehaviour
{
    public LayerMask visionLayer;
    public Transform robotHead;

    public float visionDistance;
    public float visionAngle;
    public float maxHeadRotation;
    public float scanTime;

    private RobotController robotController;
    private GameController gameController;

    private void Awake()
    {
        robotController = GetComponent<RobotController>();
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (gameController.IsGameOver)
        {
            return;
        }

        if(robotController.state == RobotController.State.PatrolState)
        {
            PatrolScan();
        }
    }

    void PatrolScan()
    {
        robotHead.localRotation = Quaternion.Euler(0f, 0f, maxHeadRotation * Mathf.Sin(Time.time * scanTime));
    }

    void InvestigateScan()
    {
        // implement investigate scan
    }

    public void HeardANoise(Vector2 atPosition)
    {
        Vector2Int noiseLocation = new Vector2Int((int)atPosition.x, (int)atPosition.y);
        Debug.DrawRay(transform.position, (Vector3)atPosition - transform.position, Color.green, 1f);
    }

    // rework this to cast a sphere and check for colliders on specific layers
    // then check to see if any items are in field of view and identify objects for reaction
    /*public void Vision(Vector2 atPosition)
    {
        Vector2 directionToTarget = atPosition - (Vector2)robotHead.position;
        float angleToTarget = Vector2.Angle(directionToTarget, robotHead.right);
        RaycastHit2D hit;

        if (angleToTarget <= visionAngle)
        {
            if (directionToTarget.magnitude <= visionDistance)
            {
                hit = Physics2D.Raycast(robotHead.position, directionToTarget.normalized, visionDistance, visionLayer);

                // was it the player?
                if (hit.transform.tag == "Player" && hit.transform.GetComponent<PlayerManager>().IsSpotted)
                {
                    Debug.Log("Hit Player");
                }
            }
        }
    } */

    // FOR TESTING ONLY
    void TestingGizmos()
    {
        Debug.DrawRay(robotHead.position, robotHead.right * visionDistance, Color.red);
        var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robotHead.right;
        var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robotHead.right;
        Debug.DrawRay(robotHead.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
        Debug.DrawRay(robotHead.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);
    }
}
