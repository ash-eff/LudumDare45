using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;
using Ash.PlayerController;

public class RobotController : MonoBehaviour
{
    public StateMachine<RobotController> stateMachine;
    public RobotController robot;
    public RobotOS robotOS;
    public RobotGUI robotGUI;
    public LayerMask visionLayer;
    public LayerMask objectLayer;

    [SerializeField] private float visionDistance;
    [SerializeField] private float visionAngle;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float scanDegrees;
    [SerializeField] private float scanSpeed;

    public int waypointIndex = 0;
    public int nextIndexInPath = 1;

    public GameObject FOV;

    public PathFinder pathfinder;
    public Animator anim;
    public SpriteRenderer robotSprite;
    public Room startingRoom;
    public PlayerController player;

    public Waypoint[] patrolWaypoints;
    public List<Vector3> path = new List<Vector3>();

    #region private
    private bool spottedPlayer;
    private bool isHacked;

    private Vector3 directionFacing;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 currentPos;
    private Vector3 currentTargetPos;
    private Vector2 destinationPosition;
    private Vector3 targetLastPosition;
    #endregion

    #region Getter and Setters
    public float VisionDistance { get { return visionDistance; } set { visionDistance = value; } }
    public float VisionAngle { get { return visionAngle; } set { visionAngle = value; } }
    public float PatrolSpeed { get { return patrolSpeed; } }
    public float ScanDegrees { get { return scanDegrees; } }
    public float ScanSpeed { get { return scanSpeed; } }

    public bool SpottedPlayer { get { return spottedPlayer; } set { spottedPlayer = value; } }
    public bool IsHacked { get { return isHacked; } set { isHacked = value; } }

    public Vector3 DirectionFacing { get { return directionFacing; } set { directionFacing = value; } }
    public Vector3 StartPos { get { return startPos; } set { startPos = value; } }
    public Vector3 EndPos { get { return endPos; } set { endPos = value; } }
    public Vector3 CurrentPos { get { return currentPos; } }
    public Vector3 CurrentTargetPos { get { return currentTargetPos; } } 
    public Vector2 DestinationPosition { get { return destinationPosition; } set { destinationPosition = value; } }
    public Vector3 TargetLastPosition { get { return targetLastPosition; } set { targetLastPosition = value; } }
    #endregion

    private void Awake()
    {
        startingRoom = GetComponentInParent<Room>();
        player = FindObjectOfType<PlayerController>();
        robot = this;
        stateMachine = new StateMachine<RobotController>(robot);
        stateMachine.ChangeState(new RobotGetGridState());

    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void SetRobotIdle(bool b)
    {
        robot.anim.SetBool("IsRobotIdle", b);
    }

    // FOR TESTING ONLY
    //void TestingGizmos()
    //{
    //    Debug.DrawRay(transform.position, robot.directionFacing * visionDistance, Color.red);
    //    var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robot.directionFacing;
    //    var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robot.directionFacing;
    //    Debug.DrawRay(transform.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
    //    Debug.DrawRay(transform.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);
    //}
}
