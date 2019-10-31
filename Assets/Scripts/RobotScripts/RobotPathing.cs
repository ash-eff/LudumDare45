using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RobotPathing : MonoBehaviour
{
    public LayerMask walkableLayer;
    public LayerMask obstacleLayer;

    public float moveSpeed;
    public float turnSpeed;

    public Waypoint[] waypoints;

    private PathFinder pathfinder;
    private RobotController robotController;

    private float currentSpeed;

    private Vector2Int startPos;
    private Vector2Int targetPos;
    private Vector2Int currentPos;
    private Vector2 currentTargetPos;

    private int waypointIndex;

    private void Start()
    {
        robotController = GetComponent<RobotController>();
        pathfinder = GetComponent<PathFinder>();
        startPos = waypoints[0].GetGridPos();
        targetPos = waypoints[1].GetGridPos();
        waypointIndex = 1;
    }

    public void StartPathing()
    {
        if(robotController.state == RobotController.State.PatrolState)
        {
            GetPathToFollow();
        }
    }

    void GetPathToFollow()
    {
        var path = pathfinder.GetPath(startPos, targetPos);
        if(path != null)
        {
            StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator FollowPath(List<Vector2Int> path)
    {
        int nextIndexInPath = 0;
        StartCoroutine(SpeedUp());
        Vector3 lastPosInList = new Vector3(path[path.Count - 1].x, path[path.Count - 1].y, 0f);

        foreach (Vector2Int vec in path)
        {
            nextIndexInPath++;
            currentPos = vec;

            if(nextIndexInPath == path.Count - 1)
            {
                float distanceToLastVec = (lastPosInList - transform.position).magnitude;
                StartCoroutine(SlowDown(distanceToLastVec));
            }

            if (nextIndexInPath > path.Count - 1)
            {
                // this is the last vec in path
            }
            else
            {
                StartCoroutine(RotateTowardsTarget(new Vector2(path[nextIndexInPath].x, path[nextIndexInPath].y)));
            }
            
            while (transform.position != new Vector3(vec.x, vec.y, 0))
            {
                transform.position = Vector2.MoveTowards(transform.position, vec, currentSpeed * Time.deltaTime);
                yield return null;
            }
        }


        yield return new WaitForSeconds(.1f);
        GetNextWaypoints();
        GetPathToFollow();
    }

    void GetNextWaypoints()
    {
        waypointIndex++;
        if (waypointIndex > waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        startPos = targetPos;
        targetPos = waypoints[waypointIndex].GetGridPos();
    }

    IEnumerator SpeedUp()
    {
        float startSpeed = .25f;
        float lerpTime = 2f;
        float currentLerpTime = 0;

        while (currentSpeed < moveSpeed)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;
            currentSpeed = Mathf.Lerp(startSpeed, moveSpeed, perc);

            yield return null;
        }
    }

    IEnumerator SlowDown(float byTime)
    {
        float startSpeed = currentSpeed;
        float endSpeed = .25f;
        float lerpTime = byTime;
        float currentLerpTime = 0;

        while (currentSpeed > endSpeed)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float perc = currentLerpTime / lerpTime;
            currentSpeed = Mathf.Lerp(startSpeed, endSpeed, perc);

            yield return null;
        }
    }

    IEnumerator RotateTowardsTarget(Vector2 target)
    {
        Vector2 targetDirection = target - (Vector2)transform.position;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * targetDirection;
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
    
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
