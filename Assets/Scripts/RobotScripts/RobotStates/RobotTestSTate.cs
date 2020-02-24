using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTestSTate : State<RobotController>
{
    //#region setup
    //private static RobotTestSTate _instance;
    //
    //private RobotTestSTate()
    //{
    //    if (_instance != null) return;
    //    _instance = this;
    //}
    //
    //public override State<RobotController> createInstance() { return Instance; }
    //
    //public static RobotTestSTate Instance
    //{
    //    get { if (_instance == null) new RobotTestSTate(); return _instance; }
    //}
    //#endregion

    RobotController robot;
    int randomInt;

    public override void EnterState(RobotController _robot)
    {
        robot = _robot;
        randomInt = Random.Range(0, 1000);
        Debug.Log(robot.name + " # " + randomInt);
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
