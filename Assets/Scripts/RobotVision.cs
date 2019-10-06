using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    public PlayerController target;
    public GameObject robot;
    public float visionDistance;
    public float visionAngle;

    private RaycastHit2D hit;
    private Vector2 directionToTarget;
    private float angleToTarget;

    private void Start()
    {
        target = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        // for testing only!
        Debug.DrawRay(robot.transform.position, robot.transform.right * visionDistance, Color.red);
        var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robot.transform.right;
        var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robot.transform.right;
        Debug.DrawRay(robot.transform.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
        Debug.DrawRay(robot.transform.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);

        directionToTarget = target.transform.position - robot.transform.position;
        angleToTarget = Vector2.Angle(directionToTarget, robot.transform.right);
        if(angleToTarget <= visionAngle)
        {
            hit = Physics2D.Raycast(robot.transform.position, directionToTarget.normalized * visionDistance);
            if (hit.transform.gameObject.tag == "Player")
            {
                Debug.Log("Player Spotted!!");
            }
        }
    }
}
