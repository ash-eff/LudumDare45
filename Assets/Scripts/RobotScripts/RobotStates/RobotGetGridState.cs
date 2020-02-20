using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGetGridState : State<RobotController>
{
    #region setup
    private static RobotGetGridState _instance;
    
    private RobotGetGridState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<RobotController> createInstance() { return Instance; }

    public static RobotGetGridState Instance
    {
        get { if (_instance == null) new RobotGetGridState(); return _instance; }
    }
    #endregion

    public override void EnterState(RobotController _robot)
    {
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
            _robot.stateMachine.ChangeState(RobotPatrolState.Instance);
        }
    }

    public override void FixedUpdateState(RobotController _robot)
    {
    }
}
