using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPatrol : MonoBehaviour
{
    //public LayerMask walkableLayer;
    //public LayerMask obstacleLayer;
    //
    //public float moveSpeed;
    //
    //public Waypoint[] waypoints;
    ////public Animator anim;
    //
    //private PathFinder pathfinder;
    //private RobotController robotController;
    //private GridMap gridMap;
    //
    //public float currentSpeed;
    //
    //public Vector3 startPos;
    //public Vector3 endPos;
    //public Vector3 currentPos;
    //private Vector3 currentTargetPos;
    //
    //public int waypointIndex = 0;
    //
    //private void Start()
    //{
    //    //anim = GetComponent<Animator>();
    //    gridMap = FindObjectOfType<GridMap>();
    //    robotController = GetComponent<RobotController>();
    //    pathfinder = GetComponent<PathFinder>();
    //}
    //
    //public void GetNextWaypoints()
    //{
    //    waypoints[waypointIndex].ResetToBaseColor();
    //    waypointIndex++;
    //    if (waypointIndex > waypoints.Length - 1)
    //    {
    //        waypointIndex = 0;
    //    }
    //    waypoints[waypointIndex].SetHighlightColor();
    //}
    //
    //public void SetPathStartAndEnd(Vector3 _start, Vector3 _end)
    //{
    //    startPos = GetLegalPosition(_start);
    //    endPos = GetLegalPosition(_end);
    //}
    //
    //public void ResetToPreviousWaypoint()
    //{
    //    waypointIndex--;
    //    if (waypointIndex < 0)
    //    {
    //        waypointIndex = 0;
    //    }
    //}
    //
    //public void GetPathToFollow()
    //{
    //    var path = pathfinder.GetPath(startPos, endPos);
    //    if (path != null)
    //    {
    //        StartCoroutine(FollowPath(path));
    //    }
    //}
    //
    //IEnumerator FollowPath(List<Vector3> path)
    //{   
    //    int nextIndexInPath = 0;
    //    StartCoroutine(SpeedUp());
    //    Vector3 lastPosInList = new Vector3(path[path.Count - 1].x, path[path.Count - 1].y, 0f);
    //    foreach (Vector3 vec in path)
    //    {
    //        int stateInt = (int)robotController.state;
    //        nextIndexInPath++;
    //        Vector2 dir = (vec - transform.position).normalized;
    //        //anim.SetFloat("DirX", dir.x);
    //        //anim.SetFloat("DirY", dir.y);
    //        currentPos = vec;
    //
    //        if (nextIndexInPath == path.Count)
    //        {
    //            float distanceToLastVec = (lastPosInList - transform.position).magnitude;
    //            StartCoroutine(SlowDown(distanceToLastVec));
    //        }
    //
    //        if (nextIndexInPath > path.Count)
    //        {
    //            // this is the last vec in path
    //        }
    //        else
    //        {
    //            //StartCoroutine(RotateTowardsTarget(new Vector3(vec.x, 0, vec.z)));
    //        }
    //
    //        while (transform.position != vec)
    //        {
    //            transform.position = Vector3.MoveTowards(transform.position, vec, currentSpeed * Time.deltaTime);
    //            yield return null;
    //        }
    //
    //        if (stateInt != (int)robotController.state)
    //        {
    //            yield return new WaitForSeconds(.25f);
    //            robotController.React();
    //            yield break;
    //        }
    //
    //    }
    //
    //    yield return new WaitForSeconds(1f);
    //
    //    if(robotController.state == RobotController.State.InvestigateState)
    //    {
    //        robotController.state = RobotController.State.PatrolState;
    //    }
    //
    //    robotController.React();
    //}
    //
    //IEnumerator SpeedUp()
    //{
    //    float startSpeed = .25f;
    //    float lerpTime = 1f;
    //    float currentLerpTime = 0;
    //
    //    while (currentSpeed < moveSpeed)
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        if (currentLerpTime > lerpTime)
    //        {
    //            currentLerpTime = lerpTime;
    //        }
    //
    //        float perc = currentLerpTime / lerpTime;
    //        currentSpeed = Mathf.Lerp(startSpeed, moveSpeed, perc);
    //
    //        yield return null;
    //    }
    //}
    //
    //IEnumerator SlowDown(float byTime)
    //{
    //    float startSpeed = currentSpeed;
    //    float endSpeed = .25f;
    //    float lerpTime = byTime;
    //    float currentLerpTime = 0;
    //
    //    while (currentSpeed > endSpeed)
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        if (currentLerpTime > lerpTime)
    //        {
    //            currentLerpTime = lerpTime;
    //        }
    //
    //        float perc = currentLerpTime / lerpTime;
    //        currentSpeed = Mathf.Lerp(startSpeed, endSpeed, perc);
    //
    //        yield return null;
    //    }
    //}
    //
    //Vector3 GetLegalPosition(Vector3 pos)
    //{
    //    Vector3[] directions = { Vector3.up, -Vector3.right, -Vector3.up, Vector3.right,
    //                             new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 1, 0) };
    //
    //    int xIntVal = (int)pos.x;
    //    int yIntVal = (int)pos.y;
    //
    //    Vector3 safePosition = new Vector3(Mathf.Abs(xIntVal) + .5f, Mathf.Abs(yIntVal) + .5f, 0f);
    //
    //    safePosition.x *= Mathf.Sign(xIntVal);
    //    safePosition.y *= Mathf.Sign(yIntVal);
    //
    //    if (!gridMap.walkableTiles.Contains(safePosition))
    //    {
    //        foreach (Vector3 dir in directions)
    //        {
    //            Vector3 checkedPos = safePosition + dir;
    //            if (pathfinder.map.ContainsKey(checkedPos))
    //            {
    //                return checkedPos;
    //            }
    //        }
    //        return transform.position;
    //    }
    //
    //    return safePosition;
    //}

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
