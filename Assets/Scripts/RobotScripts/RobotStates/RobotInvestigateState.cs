using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInvestigateState : State<RobotController>
{

    RobotController robot;

    public override void EnterState(RobotController _robot)
    {
        robot = _robot;

        float distanceToTarget = (_robot.transform.position - _robot.TargetLastPosition).magnitude;
        _robot.VisionDistance = distanceToTarget;
        _robot.VisionAngle = 5f;
    }

    public override void ExitState(RobotController _robot)
    {
    }

    public override void UpdateState(RobotController _robot)
    {
        _robot.SetRobotIdle(true);
        LookAtTarget();
        //_robot.CheckForPing();
    }

    public override void FixedUpdateState(RobotController _robot)
    {
    }


    public void LookAtTarget()
    {
        Vector3 dirToTarget = robot.TargetLastPosition - robot.FOV.transform.position;
        float angle = (Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg);
        Quaternion q = Quaternion.AngleAxis(90 - angle, Vector3.forward);
        robot.FOV.transform.rotation = Quaternion.Slerp(robot.FOV.transform.rotation, q, Time.deltaTime * 5f);
    }

}
