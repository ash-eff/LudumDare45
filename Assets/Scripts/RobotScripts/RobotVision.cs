using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    public LayerMask visionLayer;
    public LayerMask robotLayer;
    public bool inPathOfOtherRobot;
    //public PlayerManager playerTarget;
    public float visionDistance;
    public float visionAngle;
    public float maxHeadRotation;
    public float scanTime;

    public Transform robotHead;

    private GameController gameController;
    private RobotChainOfCommand chainOfCommand;
    private RobotMove robotMove;
    private RobotShoot robotShoot;
    private RaycastHit2D hit;
    private Vector2 directionToTarget;
    private MenuController menuController;
    private float angleToTarget;

    private void Awake()
    {
        chainOfCommand = FindObjectOfType<RobotChainOfCommand>();
        menuController = FindObjectOfType<MenuController>();
        gameController = FindObjectOfType<GameController>();
        robotMove = GetComponent<RobotMove>();
        robotShoot = GetComponent<RobotShoot>();
        //playerTarget = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if (gameController.IsGameOver) //|| playerTarget.IsSpotted
        {
            return;
        }

        robotHead.localRotation = Quaternion.Euler(0f, 0f, maxHeadRotation * Mathf.Sin(Time.time * scanTime));

        if (!inPathOfOtherRobot)
        {
            Debug.DrawRay(robotHead.position + robotHead.right, robotHead.right, Color.yellow);
            //hit = Physics2D.Raycast(robotHead.position + robotHead.right, robotHead.right, 1f, robotLayer);
            //if (hit)
            //{
            //    inPathOfOtherRobot = true;
            //    robotMove.AvoidRobot();
            //}
        
            //if (hit.transform.tag == "Robot")
            //{
            //    if(hit.transform.GetComponent<RobotChainOfCommand>().orderOfCommand > chainOfCommand.orderOfCommand)
            //    {
            //        
            //    }
            //    else
            //    {
            //        hit.transform.GetComponent<RobotVision>().inPathOfOtherRobot = true;
            //        hit.transform.GetComponent<RobotMove>().AvoidRobot();
            //    }
            //
            //}
        }

        // for testing only!
        Debug.DrawRay(robotHead.position, robotHead.right * visionDistance, Color.red);
        var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robotHead.right;
        var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robotHead.right;
        Debug.DrawRay(robotHead.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
        Debug.DrawRay(robotHead.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);
        
        //directionToTarget = playerTarget.transform.position - robotHead.position;
        //angleToTarget = Vector2.Angle(directionToTarget, robotHead.right);
        //if(angleToTarget <= visionAngle)
        //{
        //    if(directionToTarget.magnitude <= visionDistance)
        //    {
        //        hit = Physics2D.Raycast(robotHead.position, directionToTarget.normalized, visionDistance, visionLayer);
        //        if (hit.transform.tag == "Player" && !playerTarget.IsSpotted)
        //        {
        //            Debug.Log("Hit Player");
        //            //DispatchTarget();
        //        }
        //    }
        //}
    }

    //void DispatchTarget()
    //{      
    //    audioSource.PlayOneShot(robotCaughtYou);
    //    playerTarget.PlayerSpotted();
    //    if (playerTarget.IsDead)
    //    {
    //        robotMove.FacePlayerTarget();
    //        StartCoroutine(robotShoot.ShootTarget());
    //        playerTarget.Kill();
    //    }
    //}
}
