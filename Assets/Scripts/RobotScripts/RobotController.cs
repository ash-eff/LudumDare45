using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;
using Ash.PlayerController;

public class RobotController : MonoBehaviour
{
    public StateMachine<RobotController> stateMachine;
    public static RobotController robot;

    public LayerMask visionLayer;
    public float visionDistance;
    public float visionAngle;

    public GameObject exclaim;
    public bool spottedPlayer;
    public LayerMask objectLayer;
    public GameObject FOV;
    public Waypoint[] patrolWaypoints;
    public int waypointIndex = 0;

    public float patrolSpeed;
    public float scanDegrees;
    public float scanSpeed;

    public Vector3 directionFacing;
    public PathFinder pathfinder;
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 currentPos;
    public Vector3 currentTargetPos;
    public Vector2 destinationPosition;
    public Vector3 targetLastPosition;
    public List<Vector3> path = new List<Vector3>();
    public int nextIndexInPath = 1;

    public PlayerController playerTarget;

    private void Awake()
    {
        pathfinder = GetComponent<PathFinder>();
        playerTarget = FindObjectOfType<PlayerController>();
        robot = this;
        stateMachine = new StateMachine<RobotController>(robot);
        stateMachine.ChangeState(new RobotGetGridState());
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    // FOR TESTING ONLY
    void TestingGizmos()
    {
        Debug.DrawRay(transform.position, robot.directionFacing * visionDistance, Color.red);
        var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robot.directionFacing;
        var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robot.directionFacing;
        Debug.DrawRay(transform.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
        Debug.DrawRay(transform.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);
    }


}
