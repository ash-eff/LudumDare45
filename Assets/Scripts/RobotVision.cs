using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    public PlayerController target;
    public GameObject robot;
    public float rayLength;
    public float rayAngle;
    private Vector2 directionToTarget;
    private float angleToTarget;

    private void Start()
    {
        target = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        // for testing only!
        Debug.DrawRay(robot.transform.position, robot.transform.right * rayLength, Color.red);
        var leftDirection = Quaternion.AngleAxis(rayAngle, Vector3.forward) * robot.transform.right;
        var rightDirection = Quaternion.AngleAxis(-rayAngle, Vector3.forward) * robot.transform.right;
        Debug.DrawRay(robot.transform.position, new Vector2(rightDirection.x, rightDirection.y) * rayLength, Color.yellow);
        Debug.DrawRay(robot.transform.position, new Vector2(leftDirection.x, leftDirection.y) * rayLength, Color.blue);

        directionToTarget = target.transform.position - robot.transform.position;
        angleToTarget = Vector2.Angle(directionToTarget, robot.transform.right);
        if(angleToTarget <= rayAngle)
        {
            if(directionToTarget.magnitude <= rayLength)
            {
                Debug.Log("Target Is Spotted!!");
            }
        }
    }
}
