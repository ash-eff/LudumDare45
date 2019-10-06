using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    public LayerMask visionMask;
    public PlayerManager playerTarget;
    public GameObject robot;
    public float visionDistance;
    public float visionAngle;

    private GameController gameController;
    private RobotMove robotMove;
    private RobotShoot robotShoot;
    private RaycastHit2D hit;
    private Vector2 directionToTarget;
    private float angleToTarget;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        robotMove = GetComponent<RobotMove>();
        robotShoot = GetComponent<RobotShoot>();
        playerTarget = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if (gameController.IsGameOver)
        {
            return;
        }

        // for testing only!
        Debug.DrawRay(robot.transform.position, robot.transform.right * visionDistance, Color.red);
        var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robot.transform.right;
        var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robot.transform.right;
        Debug.DrawRay(robot.transform.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
        Debug.DrawRay(robot.transform.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);

        directionToTarget = playerTarget.transform.position - robot.transform.position;
        angleToTarget = Vector2.Angle(directionToTarget, robot.transform.right);
        if(angleToTarget <= visionAngle)
        {
            hit = Physics2D.Raycast(robot.transform.position, directionToTarget.normalized, visionDistance, visionMask);
            if (hit)
            {
                DispatchTarget();
            }
        }
    }

    void DispatchTarget()
    {
        robotMove.FacePlayerTarget();
        playerTarget.PlayerSpotted();
        StartCoroutine(robotShoot.ShootTarget());
    }
}
