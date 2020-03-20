using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIdleState : State<RobotController>
{
    RobotController robot;

    public override void EnterState(RobotController _robot)
    {
        robot = _robot;
        _robot.SetRobotIdle(true);
        // on game start there is no direction facing so the robot scans right away no matter what
        RaycastHit2D hit = Physics2D.Raycast(_robot.transform.position, _robot.DirectionFacing, _robot.VisionDistance, _robot.objectLayer);

        if (hit)
        {
            Debug.DrawRay(_robot.transform.position, _robot.DirectionFacing * _robot.VisionDistance, Color.black, 1f);
            SimpleCoroutine.Instance.StartCoroutine(WaitToMove());
        }
        else
        {
            Debug.DrawRay(_robot.transform.position, _robot.DirectionFacing * _robot.VisionDistance, Color.green, 1f);
            SimpleCoroutine.Instance.StartCoroutine(IeScanArea());
        }
    }

    public override void ExitState(RobotController _robot)
    {
    }

    public override void UpdateState(RobotController _robot)
    {
        _robot.SetRobotIdle(true);
        //LookForPlayer();
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
                //robot.robotGUI.SetExclaimActive(true);
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

    public IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(1f);
        robot.stateMachine.ChangeState(new RobotGetPathState());
    }

    public IEnumerator IeScanArea()
    {
        yield return new WaitForSeconds(.5f);
        yield return SimpleCoroutine.Instance.StartCoroutine(RotateRight(robot.ScanDegrees));
        yield return SimpleCoroutine.Instance.StartCoroutine(RotateLeft(robot.ScanDegrees * 2));
        yield return SimpleCoroutine.Instance.StartCoroutine(RotateRight(robot.ScanDegrees));
        yield return new WaitForSeconds(1f);
        robot.stateMachine.ChangeState(new RobotGetPathState());
    }

    IEnumerator RotateRight(float byAmount)
    {
        float baseRot = robot.FOV.transform.eulerAngles.z;
        float rotateTo = baseRot + byAmount;
        float currentRotation = baseRot;
        while (currentRotation < rotateTo)
        {
            currentRotation += robot.ScanSpeed * Time.deltaTime;
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, currentRotation);

            yield return null;
        }
    }

    IEnumerator RotateLeft(float byAmount)
    {
        float baseRot = robot.FOV.transform.eulerAngles.z;
        float rotateTo = baseRot - byAmount;
        float currentRotation = baseRot;
        while (currentRotation > rotateTo)
        {
            currentRotation -= robot.ScanSpeed * Time.deltaTime;
            robot.FOV.transform.rotation = Quaternion.Euler(0, 0, currentRotation);

            yield return null;
        }
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
