using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGetPathState : State<RobotController>
{
    RobotController robot;

    public override void EnterState(RobotController _robot)
    {
        robot = _robot;
        GetNextWaypoints();
        _robot.DestinationPosition = _robot.patrolWaypoints[_robot.waypointIndex].GetGridPos();
        SetPathStartAndEnd(_robot.transform.position, _robot.DestinationPosition);
        GetPathToFollow();
        _robot.stateMachine.ChangeState(new RobotPatrolState());
    }

    public override void ExitState(RobotController _robot)
    {
    }

    public override void UpdateState(RobotController _robot)
    {
        _robot.SetRobotIdle(true);
        //_robot.CheckForPing();
    }

    public override void FixedUpdateState(RobotController _robot)
    {
    }

    public void GetNextWaypoints()
    {
        robot.patrolWaypoints[robot.waypointIndex].ResetToBaseColor();
        robot.waypointIndex++;
        if (robot.waypointIndex > robot.patrolWaypoints.Length - 1)
        {
            robot.waypointIndex = 0;
        }
        robot.patrolWaypoints[robot.waypointIndex].SetHighlightColor();
    }

    public void SetPathStartAndEnd(Vector3 _start, Vector3 _end)
    {
        robot.StartPos = GetLegalPosition(_start);
        robot.EndPos = GetLegalPosition(_end);
    }

    public void GetPathToFollow()
    {
        robot.path = robot.pathfinder.GetPath(robot.StartPos, robot.EndPos);
    }

    Vector3 GetLegalPosition(Vector3 pos)
    {
        Vector3[] directions = { Vector3.up, -Vector3.right, -Vector3.up, Vector3.right,
                                 new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 1, 0) };

        int xIntVal = (int)pos.x;
        int yIntVal = (int)pos.y;

        Vector3 safePosition = new Vector3(Mathf.Abs(xIntVal) + .5f, Mathf.Abs(yIntVal) + .5f, 0f);

        safePosition.x *= Mathf.Sign(xIntVal);
        safePosition.y *= Mathf.Sign(yIntVal);

        if (!robot.pathfinder.room.walkableGrid.Contains(safePosition))
        {
            foreach (Vector3 dir in directions)
            {
                Vector3 checkedPos = safePosition + dir;
                if (robot.pathfinder.map.ContainsKey(checkedPos))
                {
                    return checkedPos;
                }
            }
            return robot.transform.position;
        }

        return safePosition;
    }
}
