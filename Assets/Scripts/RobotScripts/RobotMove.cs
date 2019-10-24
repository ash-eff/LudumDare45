using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RobotMove : MonoBehaviour
{
    public LayerMask walkableLayer;
    public LayerMask obstacleLayer;

    public float moveSpeed;
    public float turnSpeed;
    public float scanWaitTime;

    public Waypoint[] waypoints;

    private PathFinder pathfinder;
    public Vector2Int startPos;
    public Vector2Int targetPos;

    public int waypointIndex;

    //public bool getGridNow = false;

    private void Start()
    {
        pathfinder = GetComponent<PathFinder>();
        startPos = waypoints[0].GetGridPos();
        targetPos = waypoints[1].GetGridPos();
        waypointIndex = 1;
    }

    private void Update()
    {
        //if (getGridNow)
       // {
       //     getGridNow = false;
        //    GetPathToFollow();
       // }
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

    void GetNextWaypoints()
    {
        waypointIndex++;
        if(waypointIndex > waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        startPos = targetPos;
        targetPos = waypoints[waypointIndex].GetGridPos();
    }

    IEnumerator FollowPath(List<Vector2Int> path)
    {
        foreach (Vector2Int vec in path)
        {
            StartCoroutine(RotateTowardsTarget(vec));
            while (transform.position != new Vector3(vec.x, vec.y, 0))
            {
                

                transform.position = Vector2.MoveTowards(transform.position, vec, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        GetNextWaypoints();
        //yield return StartCoroutine(ScanSurroundings());
        // yield return StartCoroutine(RotateTowardsTarget(targetPos));
        //yield return new WaitForSeconds(3f);
        GetPathToFollow();
    }

    IEnumerator ScanSurroundings()
    {
        float startingRotation = transform.rotation.z;
        float maxRotation = 25f;
        float leftRotation = 0;
        float rightRotation = 0;
    
        yield return new WaitForSecondsRealtime(scanWaitTime);
    
        while(leftRotation < maxRotation)
        {
            leftRotation++;
            transform.Rotate(Vector3.forward);
            yield return null;
        }
    
        yield return new WaitForSecondsRealtime(scanWaitTime);
    
        while(rightRotation < (maxRotation * 2))
        {
            rightRotation++;
            transform.Rotate(-Vector3.forward);
            yield return null;
        }
    
        yield return new WaitForSecondsRealtime(scanWaitTime);
    
        while (transform.rotation.z < startingRotation)
        {
            transform.Rotate(Vector3.forward);
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

    //public void FacePlayerTarget()
    //{
    //    //SetTarget(playerTarget.transform);
    //    StopAllCoroutines();
    //    //StartCoroutine(RotateTowardsTarget());
    //}
}
