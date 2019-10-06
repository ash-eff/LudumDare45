using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float scanWaitTime;
    public Transform[] waypoints;
    public GameObject robot;
    private GameController gameController;
    private Transform target;
    private int currentWaypointIndex;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        currentWaypointIndex = 1;
        target = waypoints[currentWaypointIndex];
        StartCoroutine(RobotMovement());
    }

    void Update()
    {
        if (gameController.IsGameOver)
        {
            return;
        }
    }

    IEnumerator RobotMovement()
    {
        while(robot.transform.position != target.position)
        {
            robot.transform.position = Vector2.MoveTowards(robot.transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(ScanSurroundings());
    }

    IEnumerator ScanSurroundings()
    {
        float startingRotation = robot.transform.rotation.z;
        float maxRotation = 25f;
        float leftRotation = 0;
        float rightRotation = 0;

        yield return new WaitForSeconds(scanWaitTime);

        while(leftRotation < maxRotation)
        {
            leftRotation++;
            robot.transform.Rotate(Vector3.forward);
            yield return null;
        }

        yield return new WaitForSeconds(scanWaitTime);

        while(rightRotation < (maxRotation * 2))
        {
            rightRotation++;
            robot.transform.Rotate(-Vector3.forward);
            yield return null;
        }

        yield return new WaitForSeconds(scanWaitTime);

        while (robot.transform.rotation.z < startingRotation)
        {
            robot.transform.Rotate(Vector3.forward);
            yield return null;
        }

        yield return new WaitForSeconds(scanWaitTime);
        GetNextPosition();
        StartCoroutine(RotateTowardsTarget());      
    }

    IEnumerator RotateTowardsTarget()
    {
        Vector3 targetDirection = target.position - robot.transform.position;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * targetDirection;
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        while (robot.transform.rotation != targetRotation)
        {
            robot.transform.rotation = Quaternion.RotateTowards(robot.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(RobotMovement());
    }


    void GetNextPosition()
    {
        currentWaypointIndex++;
        if(currentWaypointIndex > waypoints.Length - 1)
        {
            currentWaypointIndex = 0;
        }

        target = waypoints[currentWaypointIndex];
    }
}
