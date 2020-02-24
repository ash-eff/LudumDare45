using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGetGridState : State<RobotController>
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
        if(_robot.pathfinder.grid.gridLoaded && _robot.pathfinder.isGeneratingMap)
        {
            _robot.pathfinder.GetAGridMap();
        }
        
        if (!_robot.pathfinder.isGeneratingMap)
        {
            _robot.stateMachine.ChangeState(new RobotIdleState());
        }
    }

    public override void FixedUpdateState(RobotController _robot)
    {
    }
}
