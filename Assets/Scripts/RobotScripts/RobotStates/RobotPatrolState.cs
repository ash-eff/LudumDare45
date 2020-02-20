using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPatrolState : State<RobotController>
{
    #region setup
    private static RobotPatrolState _instance;

    private RobotPatrolState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<RobotController> createInstance() { return Instance; }

    public static RobotPatrolState Instance
    {
        get { if (_instance == null) new RobotPatrolState(); return _instance; }
    }
    #endregion

    public override void EnterState(RobotController _robot)
    {
        GetNextWaypoints(_robot);
        _robot.targetPosition = _robot.waypoints[_robot.waypointIndex].GetGridPos();
        SetPathStartAndEnd(_robot, _robot.transform.position, _robot.targetPosition);
        GetPathToFollow(_robot);
    }

    public override void ExitState(RobotController _robot)
    {
    }

    public override void UpdateState(RobotController _robot)
    {
        FollowPath(_robot);
    }
    
    public override void FixedUpdateState(RobotController _robot)
    {
    }
    
    private void GetNextWaypoints(RobotController _robot)
    {
        _robot.waypoints[_robot.waypointIndex].ResetToBaseColor();
        _robot.waypointIndex++;
        if (_robot.waypointIndex > _robot.waypoints.Length - 1)
        {
            _robot.waypointIndex = 0;
        }
        _robot.waypoints[_robot.waypointIndex].SetHighlightColor();
    }
    
    private void SetPathStartAndEnd(RobotController _robot, Vector3 _start, Vector3 _end)
    {
        _robot.startPos = GetLegalPosition(_robot, _start);
        _robot.endPos = GetLegalPosition(_robot, _end);
    }
    
    Vector3 GetLegalPosition(RobotController _robot, Vector3 pos)
    {
        Vector3[] directions = { Vector3.up, -Vector3.right, -Vector3.up, Vector3.right,
                                 new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 1, 0) };
    
        int xIntVal = (int)pos.x;
        int yIntVal = (int)pos.y;
    
        Vector3 safePosition = new Vector3(Mathf.Abs(xIntVal) + .5f, Mathf.Abs(yIntVal) + .5f, 0f);
    
        safePosition.x *= Mathf.Sign(xIntVal);
        safePosition.y *= Mathf.Sign(yIntVal);
    
        if (!_robot.pathfinder.grid.walkableTiles.Contains(safePosition))
        {
            foreach (Vector3 dir in directions)
            {
                Vector3 checkedPos = safePosition + dir;
                if (_robot.pathfinder.map.ContainsKey(checkedPos))
                {
                    return checkedPos;
                }
            }
            return _robot.transform.position;
        }
    
        return safePosition;
    }
    
    private void GetPathToFollow(RobotController _robot)
    {
        _robot.path = _robot.pathfinder.GetPath(_robot.startPos, _robot.endPos);
    }
    
    private void FollowPath(RobotController _robot)
    {
        
        if(_robot.nextIndexInPath < _robot.path.Count)
        {
            _robot.directionFacing = (_robot.path[_robot.nextIndexInPath] - _robot.transform.position).normalized;
            _robot.transform.position = Vector3.MoveTowards(_robot.transform.position, _robot.path[_robot.nextIndexInPath], _robot.patrolSpeed * Time.deltaTime);
    
            if(_robot.transform.position == _robot.path[_robot.nextIndexInPath])
            {
                _robot.nextIndexInPath++;
            }
        }
        else
        {
            _robot.nextIndexInPath = 1;
            _robot.stateMachine.ChangeState(RobotPatrolState.Instance);
        }
    }
}
