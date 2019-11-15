using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPatrol : MonoBehaviour
{
    public LayerMask walkableLayer;
    public LayerMask obstacleLayer;

    public float moveSpeed;

    public Waypoint[] waypoints;

    private PathFinder pathfinder;
    private RobotController robotController;

    private float currentSpeed;

    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 currentPos;
    private Vector3 currentTargetPos;

    public int waypointIndex;

    private void Start()
    {
        robotController = GetComponent<RobotController>();
        pathfinder = GetComponent<PathFinder>();
    }

    public void GetNextWaypoints(Vector3 currPos)
    {
        waypointIndex++;
        if (waypointIndex > waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        SetPathStartAndEnd(currPos, waypoints[waypointIndex].transform.position);
    }

    public void SetPathStartAndEnd(Vector3 _start, Vector3 _end)
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
        if (path != null)
        {
            StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator FollowPath(List<Vector3> path)
    {
        int stateValue = (int)robotController.state;
        int nextIndexInPath = 0;
        StartCoroutine(SpeedUp());
        Vector3 lastPosInList = new Vector3(path[path.Count - 1].x, 0, path[path.Count - 1].z);

        foreach (Vector3 vec in path)
        {
            nextIndexInPath++;
            currentPos = vec;

            if (nextIndexInPath == path.Count - 1)
            {
                float distanceToLastVec = (lastPosInList - new Vector3(transform.position.x, 0f, transform.position.z)).magnitude;
                StartCoroutine(SlowDown(distanceToLastVec));
            }

            if (nextIndexInPath > path.Count - 1)
            {
                // this is the last vec in path
            }
            else
            {
               // StartCoroutine(RotateTowardsTarget(new Vector3(vec.x, 0, vec.z)));
            }

            while (new Vector3(transform.position.x, 0f, transform.position.z) != new Vector3(vec.x, 0, vec.z))
            {
                transform.position = Vector3.MoveTowards(transform.position, vec, currentSpeed * Time.deltaTime);
                yield return null;
            }

            if (stateValue != (int)robotController.state)
            {
                robotController.React();
                yield break;
            }
        }

        if (robotController.state == RobotController.State.ReturnState)
        {
            robotController.ItemToInvestigate.GetComponent<ValuableItem>().PlaceItem();
            robotController.state = RobotController.State.PatrolState;
        }
        robotController.React();
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

    //public IEnumerator RotateTowardsTarget(Vector2 target)
    //{
    //    float lerpTime = .2f;
    //    float currentLerpTime = 0;
    //    float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
    //    float startingRot = transform.localEulerAngles.z;
    //
    //    if (startingRot > 180)
    //    {
    //        startingRot -= 360f;
    //    }
    //
    //    while (startingRot != Mathf.Abs(angle))
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        float perc = currentLerpTime / lerpTime;
    //        float diff = Mathf.LerpAngle(startingRot, angle, perc);
    //        transform.rotation = Quaternion.Euler(0f, 0f, diff);
    //
    //        yield return null;
    //    }
    //}
}
