using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIdleState : State<RobotController>
{
    #region setup
    private static RobotIdleState _instance;

    private RobotIdleState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<RobotController> createInstance() { return Instance; }

    public static RobotIdleState Instance
    {
        get { if (_instance == null) new RobotIdleState(); return _instance; }
    }
    #endregion

    public override void EnterState(RobotController robot)
    {
    }

    public override void ExitState(RobotController robot)
    {
    }

    public override void UpdateState(RobotController robot)
    {
        Debug.Log(robot.transform.name + " IDLE");
    }

    public override void FixedUpdateState(RobotController robot)
    {
    }
}
