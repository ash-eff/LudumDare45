using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPatrolState : State<RobotController>
{
    RobotController robot;

    public override void EnterState(RobotController _robot)
    {
        robot = _robot;
    }

    public override void ExitState(RobotController _robot)
    {
    }

    public override void UpdateState(RobotController _robot)
    {
        LookForPlayer();
        FollowPath();
        RotateVision();
    }
    
    public override void FixedUpdateState(RobotController _robot)
    {
    }

    public void LookForPlayer()
    {
        Vector2[] directionsToTargets = { GetDirectionToTarget(robot.playerTarget.feet.transform.position),
                          GetDirectionToTarget(robot.playerTarget.head.transform.position),
                          GetDirectionToTarget(robot.playerTarget.transform.position) };

        if (IsTargetSeen(directionsToTargets))
        {
            if (!robot.spottedPlayer)
            {
                robot.spottedPlayer = true;
                robot.exclaim.SetActive(true);
                robot.playerTarget.timesSpotted++;
                robot.targetLastPosition = robot.playerTarget.transform.position;
                robot.stateMachine.ChangeState(new RobotInvestigateState());
            }
        }

        else
        {
            robot.spottedPlayer = false;
            robot.exclaim.SetActive(false);
        }
    }

    public void FollowPath()
    {
        if (robot.nextIndexInPath < robot.path.Count)
        {
            robot.directionFacing = (robot.path[robot.nextIndexInPath] - robot.transform.position).normalized;
            robot.transform.position = Vector3.MoveTowards(robot.transform.position, robot.path[robot.nextIndexInPath], robot.patrolSpeed * Time.deltaTime);

            if (robot.transform.position == robot.path[robot.nextIndexInPath])
            {
                robot.nextIndexInPath++;
            }
        }
        else
        {
            robot.nextIndexInPath = 1;
            robot.stateMachine.ChangeState(new RobotIdleState());
        }
    }
    // rewrite this to be smooth
    public void RotateVision()
    {
        if (robot.directionFacing == Vector3.right)
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, 90);
        if (robot.directionFacing == -Vector3.right)
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, -90);
        if (robot.directionFacing == Vector3.up)
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (robot.directionFacing == -Vector3.up)
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    private Vector3 GetDirectionToTarget(Vector3 _location)
    {
        Vector2 directionToTarget = _location - robot.transform.position;
        return directionToTarget;
    }

    private bool IsTargetSeen(Vector2[] _directionsToTargets)
    {
        if (robot.playerTarget.isStealthed)
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
        float angleToTarget = Vector2.Angle(robot.directionFacing, _direction);
        if (angleToTarget <= robot.visionAngle && _direction.magnitude <= robot.visionDistance)
        {
            return true;
        }
        return false;
    }

    private bool TargetInLineOfSight(Vector2 _target)
    {
        RaycastHit2D hit = Physics2D.Raycast(robot.transform.position, _target.normalized, robot.visionDistance, robot.visionLayer);
        if (hit)
        {
            if (hit.collider.tag == "Player" || hit.collider.tag == "PlayerVisualTrigger")
            {
                Debug.DrawRay(robot.transform.position, _target, Color.green);
                return true;
            }

            Debug.DrawRay(robot.transform.position, _target, Color.red);
            return false;
        }

        return false;
    }
}
