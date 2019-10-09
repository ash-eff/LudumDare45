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
    private MenuController menuController;
    private float angleToTarget;

    public AudioSource audioSource;
    public AudioClip robotCaughtYou;

    private void Awake()
    {
        menuController = FindObjectOfType<MenuController>();
        gameController = FindObjectOfType<GameController>();
        robotMove = GetComponent<RobotMove>();
        robotShoot = GetComponent<RobotShoot>();
        playerTarget = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if (gameController.IsGameOver || playerTarget.IsSpotted)
        {
            return;
        }

        audioSource.volume = menuController.SFXVolume;

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
            if(directionToTarget.magnitude <= visionDistance)
            {
                hit = Physics2D.Raycast(robot.transform.position, directionToTarget.normalized, visionDistance, visionMask);
                if (hit.transform.tag == "Player" && !playerTarget.IsSpotted)
                {
                    DispatchTarget();
                }
            }
        }
    }

    void DispatchTarget()
    {      
        audioSource.PlayOneShot(robotCaughtYou);
        playerTarget.PlayerSpotted();
        if (playerTarget.IsDead)
        {
            robotMove.FacePlayerTarget();
            StartCoroutine(robotShoot.ShootTarget());
            playerTarget.Kill();
        }
    }
}
