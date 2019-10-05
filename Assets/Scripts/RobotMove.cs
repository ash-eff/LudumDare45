using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{
    public float moveSpeed;
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
        Debug.Log("Scanning Area");
        yield return new WaitForSeconds(2f);
        GetNextPosition();
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
