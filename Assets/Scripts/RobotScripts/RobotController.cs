using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;

public class RobotController : MonoBehaviour
{
    public StateMachine<RobotController> stateMachine;
    public static RobotController robot;

    public Waypoint[] waypoints;
    public int waypointIndex = 0;

    public float patrolSpeed;

    public Vector3 directionFacing;

    public PathFinder pathfinder;
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 currentPos;
    public Vector3 currentTargetPos;
    public Vector2 targetPosition;
    public List<Vector3> path = new List<Vector3>();
    public int nextIndexInPath = 1;
    public int rand;

    private void Awake()
    {
        rand = Random.Range(0, 100);
        robot = this;
        stateMachine = new StateMachine<RobotController>(robot);
        stateMachine.ChangeState(RobotGetGridState.Instance);
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();
}
