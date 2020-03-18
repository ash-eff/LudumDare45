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
        RaycastHit2D hit = Physics2D.Raycast(_robot.transform.position, _robot.directionFacing, _robot.visionDistance, _robot.objectLayer);

        if (hit)
        {
            Debug.DrawRay(_robot.transform.position, _robot.directionFacing * _robot.visionDistance, Color.black, 1f);
            SimpleCoroutine.Instance.StartCoroutine(WaitToMove());
        }
        else
        {
            Debug.DrawRay(_robot.transform.position, _robot.directionFacing * _robot.visionDistance, Color.green, 1f);
            SimpleCoroutine.Instance.StartCoroutine(IeScanArea());
        }
    }

    public override void ExitState(RobotController _robot)
    {
    }

    public override void UpdateState(RobotController _robot)
    {
        _robot.SetRobotIdle(true);
        LookForPlayer();
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

    public IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(1f);
        robot.stateMachine.ChangeState(new RobotGetPathState());
    }

    public IEnumerator IeScanArea()
    {
        yield return new WaitForSeconds(.5f);
        yield return SimpleCoroutine.Instance.StartCoroutine(RotateRight(robot.scanDegrees));
        yield return SimpleCoroutine.Instance.StartCoroutine(RotateLeft(robot.scanDegrees * 2));
        yield return SimpleCoroutine.Instance.StartCoroutine(RotateRight(robot.scanDegrees));
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
            currentRotation += robot.scanSpeed * Time.deltaTime;
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
            currentRotation -= robot.scanSpeed * Time.deltaTime;
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
