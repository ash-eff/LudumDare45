using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{
    public LayerMask walkableLayer;
    public LayerMask obstacleLayer;
    public int maxForwardDistance;
    public int leftRayDist, rightRayDist, forwardRayDist;
    [Range(0,1)]
    public float randomTurnChance;
    public float moveSpeed;
    public float turnSpeed;
    public float scanWaitTime;
    public PathFinder pathfinder;
    public Node startNode;
    public Node targetNode;

    public bool wallInFront;
    public bool wallToLeft;
    public bool wallToRight;

    RaycastHit2D forwardHit;
    RaycastHit2D leftHit;
    RaycastHit2D rightHit; 

    private void Start()
    {
        GetPathToFollow();
    }

    private void Update()
    {
        CheckWalls();
    }

    void CheckWalls()
    {
        Debug.DrawRay(transform.position, transform.right * forwardRayDist, Color.red);
        Debug.DrawRay(transform.position, transform.up * leftRayDist, Color.green);
        Debug.DrawRay(transform.position, -transform.up * rightRayDist, Color.blue);

        forwardHit = Physics2D.Raycast(transform.position, transform.right, forwardRayDist, obstacleLayer);
        leftHit = Physics2D.Raycast(transform.position, transform.up, leftRayDist, obstacleLayer);
        rightHit = Physics2D.Raycast(transform.position, -transform.up, rightRayDist, obstacleLayer);

        if (forwardHit)
        {
            wallInFront = true;
        }
        else
        {
            wallInFront = false;
        }

        if (rightHit)
        {
            wallToRight = true;
        }
        else
        {
            wallToRight = false;
        }

        if (leftHit)
        {
            wallToLeft = true;
        }
        else
        {
            wallToLeft = false;
        }
    }

    void GetPathToFollow()
    {
        startNode = GetNode(transform.position);
        targetNode = GetNode(transform.position + (transform.right * maxForwardDistance));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxForwardDistance, obstacleLayer);
        if (hit)
        {
            targetNode = GetNode(hit.point);
        }

        var path = pathfinder.GetPath(startNode, targetNode);
        StartCoroutine(FollowPath(path));
    }

    IEnumerator FollowPath(List<Node> path)
    {
        Vector2 newTarget;

        foreach (Node node in path)
        {
            while (transform.position != node.transform.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, node.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            if (wallInFront)
            {
                newTarget = GetNewDirection();
                yield return StartCoroutine(RotateTowardsTarget(newTarget));
                yield return StartCoroutine(ScanSurroundings());
                GetPathToFollow();
            }

            if(!wallToRight || !wallToLeft)
            {
                if (CheckRedirectChance())
                {                 
                    StartCoroutine(MakeRandomTurn());
                    yield break;
                }
            }
        }

        yield return StartCoroutine(ScanSurroundings());
        newTarget = GetNewDirection();
        yield return StartCoroutine(RotateTowardsTarget(newTarget));
        GetPathToFollow();
    }

    bool CheckRedirectChance()
    {
        float chanceToTurn = Random.value;
        if (chanceToTurn <= randomTurnChance)
        {
            return true;
        }

        return false;
    }

    IEnumerator MakeRandomTurn()
    {
        bool turnRight = false;

        if(!wallToRight && !wallToLeft)
        {
            turnRight = Random.value > .5 ? true : false;
        }
        else if (!wallToRight)
        {
            turnRight = true;
        }
        else
        {
            turnRight = false;
        }
      
        if (turnRight)
        {
            yield return StartCoroutine(ScanSurroundings());
            Vector2 newTarget;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, maxForwardDistance, obstacleLayer);
            if (hit)
            {
                newTarget = GetNode(hit.point).GetGridPos();
            }
            else
            {
                newTarget = GetNode(transform.position + (-transform.up * maxForwardDistance)).GetGridPos();
            }

            yield return StartCoroutine(RotateTowardsTarget(newTarget));

            GetPathToFollow();
        }
        else
        {
            yield return StartCoroutine(ScanSurroundings());
            Vector2 newTarget;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxForwardDistance, obstacleLayer);
            if (hit)
            {
                newTarget = GetNode(hit.point).GetGridPos();
            }
            else
            {
                newTarget = GetNode(transform.position + (transform.up * maxForwardDistance)).GetGridPos();
            }

            yield return StartCoroutine(RotateTowardsTarget(newTarget));

            GetPathToFollow();
        }
    }

    Vector2 GetNewDirection()
    {
        Vector2 newDirection;
        Vector3 directionToCast = Vector2.zero;
        float chance = Random.value;

        if (!wallToRight && !wallToLeft)
        {       
            if(chance <= .1f)
            {
                directionToCast = -transform.right;
            }
            else if(chance >= .55f)
            {
                directionToCast = transform.up;
            }
            else
            {
                directionToCast = -transform.up;
            }
        }

        if(!wallToRight && wallToLeft)
        {
            if(chance <= .1f)
            {
                directionToCast = -transform.right;
            }
            else
            {
                directionToCast = -transform.up;
            }
        }

        if (!wallToLeft && wallToRight)
        {
            if (chance <= .1f)
            {
                directionToCast = -transform.right;
            }
            else
            {
                directionToCast = transform.up;
            }
        }

        if (wallToRight && wallToLeft)
        {
            directionToCast = -transform.right;
        }


        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToCast, maxForwardDistance, obstacleLayer);
        if (hit)
        {
            newDirection = GetNode(hit.point).GetGridPos();
        }
        else
        {
            newDirection = GetNode(transform.position + (directionToCast * maxForwardDistance)).GetGridPos();
        }

        return newDirection;
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

   //void GetNextWaypoints(Waypoint currentPosition)
   //{
   //    List<Waypoint> availableWaypoints = new List<Waypoint>();
   //    foreach(Waypoint waypoint in waypoints)
   //    {
   //        if(waypoint != currentPosition)
   //        {
   //            availableWaypoints.Add(waypoint);
   //        }
   //    }
   //
   //    startingWaypoint = currentPosition;
   //    targetWaypoint = Random.value > .5f ? targetWaypoint = availableWaypoints[0] : targetWaypoint = availableWaypoints[1];
   //}

    public void FacePlayerTarget()
    {
        //SetTarget(playerTarget.transform);
        StopAllCoroutines();
        //StartCoroutine(RotateTowardsTarget());
    }

    Node GetNode(Vector2 atLocation)
    {
        RaycastHit2D hit = Physics2D.CircleCast(atLocation, .25f, Vector2.zero, 0, walkableLayer);
        if (hit)
        {
            Node node = hit.transform.gameObject.GetComponent<Node>();
            return node;
        }

        return null;
    }
}
