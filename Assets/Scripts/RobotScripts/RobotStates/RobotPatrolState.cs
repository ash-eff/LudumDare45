using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPatrolState : State<RobotController>
{
    RobotController robot;

    public override void EnterState(RobotController _robot)
    {
        robot = _robot;
        _robot.SetRobotIdle(false);
    }

    public override void ExitState(RobotController _robot)
    {
    }

    public override void UpdateState(RobotController _robot)
    {
        _robot.SetRobotIdle(false);
        //LookForPlayer();
        FollowPath();
        RotateVision();
        //_robot.CheckForPing();
    }
    
    public override void FixedUpdateState(RobotController _robot)
    {
    }

    public void LookForPlayer()
    {
        Vector2[] directionsToTargets = { GetDirectionToTarget(robot.player.feet.transform.position),
                          GetDirectionToTarget(robot.player.head.transform.position),
                          GetDirectionToTarget(robot.player.transform.position) };

        if (IsTargetSeen(directionsToTargets))
        {
            if (!robot.SpottedPlayer)
            {
                robot.SpottedPlayer = true;
                robot.robotGUI.SetExclaimActive(true);
                robot.player.timesSpotted++;
                robot.TargetLastPosition = robot.player.transform.position;
                robot.stateMachine.ChangeState(new RobotInvestigateState());
            }
        }

        else
        {
            robot.SpottedPlayer = false;
            //robot.robotGUI.SetExclaimActive(false);
        }
    }

    public void FollowPath()
    {
        if (robot.nextIndexInPath < robot.path.Count)
        {
            robot.DirectionFacing = (robot.path[robot.nextIndexInPath] - robot.transform.position).normalized;
            if(robot.DirectionFacing.x != 0)
            {
                robot.robotSprite.transform.localScale = new Vector2(robot.DirectionFacing.x, 1);
            }
            else
            {
                robot.robotSprite.transform.localScale = new Vector2(1, 1);
            }

            //Debug.Log(robot.DirectionFacing);
            robot.transform.position = Vector3.MoveTowards(robot.transform.position, robot.path[robot.nextIndexInPath], robot.PatrolSpeed * Time.deltaTime);

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
        if (robot.DirectionFacing == Vector3.right)
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, 90);
        if (robot.DirectionFacing == -Vector3.right)
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, -90);
        if (robot.DirectionFacing == Vector3.up)
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, 0); 
        if (robot.DirectionFacing == -Vector3.up)
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    private Vector3 GetDirectionToTarget(Vector3 _location)
    {
        Vector2 directionToTarget = _location - robot.transform.position;
        return directionToTarget;
    }

    private bool IsTargetSeen(Vector2[] _directionsToTargets)
    {
        if (robot.player.isStealthed)
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
        float angleToTarget = Vector2.Angle(robot.DirectionFacing, _direction);
        if (angleToTarget <= robot.VisionAngle && _direction.magnitude <= robot.VisionDistance)
        {
            return true;
        }
        return false;
    }

    private bool TargetInLineOfSight(Vector2 _target)
    {
        RaycastHit2D hit = Physics2D.Raycast(robot.transform.position, _target.normalized, robot.VisionDistance, robot.visionLayer);
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
