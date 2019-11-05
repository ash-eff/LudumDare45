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

    public Vector2Int startPos;
    public Vector2Int endPos;
    public Vector2Int currentPos;
    private Vector2 currentTargetPos;

    public GameObject itemBeingIvestigated;

    public int waypointIndex;

    private void Start()
    {
        robotController = GetComponent<RobotController>();
        pathfinder = GetComponent<PathFinder>();
    }

    public void GetNextWaypoints(Vector2Int currPos)
    {
        waypointIndex++;
        if (waypointIndex > waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        SetPathStartAndEnd(currPos, waypoints[waypointIndex].GetGridPos());
    }

    public void SetPathStartAndEnd(Vector2Int _start, Vector2Int _end)
    {
        startPos = _start;
        endPos = _end;
    }

    public void ResetToPreviousWaypoint()
    {
        waypointIndex--;
        if (waypointIndex < 0)
        {
            waypointIndex = 0;
        }
    }

    public void GetPathToFollow()
    {
        var path = pathfinder.GetPath(startPos, endPos);
        if(path != null)
        {
            StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator FollowPath(List<Vector2Int> path)
    {
        int stateValue = (int)robotController.state;
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

            if(stateValue != (int)robotController.state)
            {
                robotController.React();
                yield break;
            }

            if(robotController.state == RobotController.State.InvestigateState)
            {
                StartCoroutine(DealWithInvestigation());
                break;
            }
        }

        robotController.React();
    }

    IEnumerator DealWithInvestigation()
    {
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator InvestigatePath(Vector2Int _start, Vector2Int _end)
    {
        robotController.state = RobotController.State.InvestigateState;
        yield return new WaitForSeconds(1f);
        SetPathStartAndEnd(_start, _end);
        GetPathToFollow();
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

    public IEnumerator RotateTowardsTarget(Vector2 target)
    {
        float lerpTime = .5f;
        float currentLerpTime = 0;
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        float startingRot = transform.localEulerAngles.z;

        if(startingRot > 180)
        {
            startingRot -= 360f;
        }

        while (startingRot != Mathf.Abs(angle))
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            float diff = Mathf.LerpAngle(startingRot, angle, perc);
            transform.rotation = Quaternion.Euler(0f, 0f, diff);
    
            yield return null;
        }      
    }
}
