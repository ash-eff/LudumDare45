using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public enum State { PatrolState, SearchState, InvestigateState, AlertState, WaitState, BreakState }
    public State state;

    private PathFinder pathfinder;
    private RobotPathing robotPathing;
    private RobotSenses robotSenses;
    private RobotAlert robotAlert;

    private void Start()
    {
        robotPathing = GetComponent<RobotPathing>();
        robotSenses = GetComponent<RobotSenses>();
        robotAlert = GetComponent<RobotAlert>();
        pathfinder = GetComponent<PathFinder>();
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    {
        while (pathfinder.isGeneratingMap)
        {
            yield return null;
        }

        state = State.PatrolState;
        robotPathing.GetNextWaypoints(GetVector2IntOfPosition(transform.position));
        robotPathing.GetPathToFollow();
    }

    public void React()
    {
        switch (state)
        {
            case State.PatrolState:
                robotPathing.GetNextWaypoints(GetVector2IntOfPosition(transform.position));
                robotPathing.GetPathToFollow();
                break;

            case State.AlertState:
                Vector2 investigatePosition = robotSenses.locationOfSuspicion;
                StartCoroutine(robotPathing.RotateTowardsTarget(investigatePosition));
                robotAlert.OnAlert();
                StartCoroutine(robotPathing.InvestigatePath(GetVector2IntOfPosition(transform.position), GetVector2IntOfPosition(investigatePosition)));
                break;

            case State.InvestigateState:
                Debug.Log("Clear");
                state = State.PatrolState;
                robotAlert.ReturnToStatusQuo();
                robotSenses.lockHeadOnTarget = false;
                robotPathing.ResetToPreviousWaypoint();
                robotPathing.GetNextWaypoints(GetVector2IntOfPosition(transform.position));
                robotPathing.GetPathToFollow();
                break;
        }
    }

    Vector2Int GetVector2IntOfPosition(Vector3 ofPosition)
    {
        return new Vector2Int((int)ofPosition.x, (int)ofPosition.y);
    }
}
