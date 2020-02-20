using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class RobotSenses : MonoBehaviour
{
    public LayerMask visionLayer;
    public float visionDistance;
    public float visionAngle;

    public GameObject exclaim;

    public  PlayerController player;
    private RobotController robot;
    
    private void Awake()
    {
        robot = GetComponent<RobotController>();
        player = FindObjectOfType<PlayerController>();
    }
    
    private void Start()
    {
        StartCoroutine(Vision());
    }
    
    void Update()
    {
        TestingGizmos();
    }
    
    public IEnumerator Vision()
    {
        while(true)
        {
            Vector2[] directionsToTargets = { GetDirectionToTarget(player.feet.transform.position),
                              GetDirectionToTarget(player.head.transform.position),
                              GetDirectionToTarget(player.transform.position) };

            if (TargetIsSeen(directionsToTargets))
                exclaim.SetActive(true);
            else
                exclaim.SetActive(false);

            yield return null;
        }
    }

    private Vector3 GetDirectionToTarget(Vector3 _location)
    {
        Vector2 directionToTarget = _location - transform.position;
        return directionToTarget;
    }

    private bool TargetIsSeen(Vector2[] _directionsToTargets)
    {
        if (player.isStealthed)
            return false;

        int numberOfTargetsSeen = 0;

        foreach (Vector2 target in _directionsToTargets)
        {
            if (TargetInVisionCone(target))
            {
                if (TargetInLineOfSight(target))
                {
                    numberOfTargetsSeen++;
                }
            }
        }

        if (numberOfTargetsSeen > 0)
            return true;
        else
            return false;
    }

    private bool TargetInVisionCone(Vector2 _direction)
    {
        float angleToTarget = Vector2.Angle(transform.right, _direction);
        if(angleToTarget <= visionAngle && _direction.magnitude <= visionDistance)
        {
            return true;
        }
        return false;
    }

    private bool TargetInLineOfSight(Vector2 _target)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _target.normalized, visionDistance, visionLayer);
        

        if (hit)
        {
            if (hit.collider.tag == "Player" || hit.collider.tag == "PlayerVisualTrigger")
            {
                Debug.DrawRay(transform.position, _target, Color.green);
                return true;
            }

            Debug.DrawRay(transform.position, _target, Color.red);
            return false;
        }

        return false;
    }
    
    // FOR TESTING ONLY
    void TestingGizmos()
    {
        Debug.DrawRay(transform.position, robot.directionFacing * visionDistance, Color.red);
        var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robot.directionFacing;
        var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robot.directionFacing;
        Debug.DrawRay(transform.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
        Debug.DrawRay(transform.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, visionDistance);
    }
}
