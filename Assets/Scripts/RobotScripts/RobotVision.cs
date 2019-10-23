using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotVision : MonoBehaviour
{
    public LayerMask visionLayer;
    public LayerMask robotLayer;
    public bool inPathOfOtherRobot;
    public PlayerManager playerTarget;
    public float visionDistance;
    public float visionAngle;

    private GameController gameController;
    private RobotChainOfCommand chainOfCommand;
    private RobotMove robotMove;
    private RobotShoot robotShoot;
    private RaycastHit2D hit;
    private Vector2 directionToTarget;
    private MenuController menuController;
    private float angleToTarget;

    public AudioSource audioSource;
    public AudioClip robotCaughtYou;

    private void Awake()
    {
        chainOfCommand = FindObjectOfType<RobotChainOfCommand>();
        menuController = FindObjectOfType<MenuController>();
        gameController = FindObjectOfType<GameController>();
        robotMove = GetComponent<RobotMove>();
        robotShoot = GetComponent<RobotShoot>();
        playerTarget = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if (gameController.IsGameOver) //|| playerTarget.IsSpotted
        {
            return;
        }

        audioSource.volume = menuController.SFXVolume;

        //if (!inPathOfOtherRobot)
        //{
        //    Debug.DrawRay(robot.transform.position + robot.transform.right, robot.transform.right, Color.yellow);
        //    hit = Physics2D.Raycast(robot.transform.position + robot.transform.right, robot.transform.right, 1f, robotLayer);
        //    if (hit)
        //    {
        //        inPathOfOtherRobot = true;
        //        robotMove.AvoidRobot();
        //    }
        //
        //    //if (hit.transform.tag == "Robot")
        //    //{
        //    //    if(hit.transform.GetComponent<RobotChainOfCommand>().orderOfCommand > chainOfCommand.orderOfCommand)
        //    //    {
        //    //        
        //    //    }
        //    //    else
        //    //    {
        //    //        hit.transform.GetComponent<RobotVision>().inPathOfOtherRobot = true;
        //    //        hit.transform.GetComponent<RobotMove>().AvoidRobot();
        //    //    }
        //    //
        //    //}
        //}

        //// for testing only!
        //Debug.DrawRay(robot.transform.position, robot.transform.right * visionDistance, Color.red);
        //var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robot.transform.right;
        //var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robot.transform.right;
        //Debug.DrawRay(robot.transform.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
        //Debug.DrawRay(robot.transform.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);
        //
        //directionToTarget = playerTarget.transform.position - robot.transform.position;
        //angleToTarget = Vector2.Angle(directionToTarget, robot.transform.right);
        //if(angleToTarget <= visionAngle)
        //{
        //    if(directionToTarget.magnitude <= visionDistance)
        //    {
        //        hit = Physics2D.Raycast(robot.transform.position, directionToTarget.normalized, visionDistance, visionLayer);
        //        if (hit.transform.tag == "Player" && !playerTarget.IsSpotted)
        //        {
        //            DispatchTarget();
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
