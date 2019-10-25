using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RobotMove : MonoBehaviour
{
    public LayerMask walkableLayer;
    public LayerMask obstacleLayer;

    public float moveSpeed;
    public float currentSpeed;
    public float turnSpeed;
    public float scanWaitTime;

    public Waypoint[] waypoints;

    private PathFinder pathfinder;
    public Vector2Int startPos;
    public Vector2Int targetPos;

    public Vector2 currentTargetPos;

    public int waypointIndex;

    private void Start()
    {
        pathfinder = GetComponent<PathFinder>();
        startPos = waypoints[0].GetGridPos();
        targetPos = waypoints[1].GetGridPos();
        waypointIndex = 1;
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    {
        while (pathfinder.isGeneratingMap)
        {
            yield return null;
        }

        GetPathToFollow();
    }

    public void GetPathToFollow()
    {
        var path = pathfinder.GetPath(startPos, targetPos);
        if(path.Count == 0)
        {
            Debug.LogWarning("No path returned");
        }
        else
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
            

            if(nextIndexInPath == path.Count - 1)
            {
                float distanceToLastVec = (lastPosInList - transform.position).magnitude;
                Debug.Log("Dist: " + distanceToLastVec);
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
        Debug.Log("Speed Up");
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
        Debug.Log("Slow Down");
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

    void RotateBody(Vector3 toTarget)
    {
        Vector3 targetDirection = toTarget - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turnSpeed);
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

    //public void FacePlayerTarget()
    //{
    //    //SetTarget(playerTarget.transform);
    //    StopAllCoroutines();
    //    //StartCoroutine(RotateTowardsTarget());
    //}
}
