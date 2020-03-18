using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotExplodeState : State<RobotController>
{
    public override void EnterState(RobotController _robot)
    {
        _robot.gameObject.SetActive(false);
    }

    public override void ExitState(RobotController _robot)
    {
    }

    public override void UpdateState(RobotController _robot)
    {
    }

    public override void FixedUpdateState(RobotController _robot)
    {
    }
}
