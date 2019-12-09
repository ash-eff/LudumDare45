using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public enum State { PatrolState, SearchState, InvestigateState, ReturnState, AlarmState, WaitState, BreakState }
    public State state;

    public Vector2 targetPosition;
    private PathFinder pathfinder;
    private RobotPatrol robotPatrol;
    private RobotSenses robotSenses;
    //private RobotAlert robotAlert;
    private RobotInvestigate robotInvestigate;

    private GameObject itemOfInvestigation;

    public GameObject ItemToInvestigate
    {
        get { return itemOfInvestigation; }
        set { itemOfInvestigation = value; }
    }

    private void Start()
    {
        robotInvestigate = GetComponent<RobotInvestigate>();
        robotPatrol = GetComponent<RobotPatrol>();
        robotSenses = GetComponent<RobotSenses>();
        //robotAlert = GetComponent<RobotAlert>();
        pathfinder = GetComponent<PathFinder>();
        StartCoroutine(WaitToStart());
        //state = State.PatrolState;
    }

    IEnumerator WaitToStart()
    {
        while (pathfinder.isGeneratingMap)
        {
            yield return null;
        }
    
        state = State.PatrolState;
        robotPatrol.GetNextWaypoints();
        targetPosition = robotPatrol.waypoints[robotPatrol.waypointIndex].GetGridPos();
        robotPatrol.SetPathStartAndEnd(transform.position, targetPosition);
        robotPatrol.GetPathToFollow();
    }

    public void React()
    {
        switch (state)
        {         
            case State.PatrolState:
                //robotSenses.lockHeadOnTarget = false;
                //robotAlert.ReturnToStatusQuo();
                //StartCoroutine(robotSenses.Vision());
                robotPatrol.GetNextWaypoints();
                targetPosition = robotPatrol.waypoints[robotPatrol.waypointIndex].GetGridPos();
                robotPatrol.SetPathStartAndEnd(transform.position, targetPosition);
                robotPatrol.GetPathToFollow();
                break;

            case State.InvestigateState:                
                Vector2 investigatePosition = robotSenses.locationOfSuspicion;
                //StartCoroutine(robotPatrol.RotateTowardsTarget(investigatePosition));
                //robotAlert.OnAlert();
                //robotPatrol.ResetToPreviousWaypoint();
                robotPatrol.SetPathStartAndEnd(transform.position, investigatePosition);
                robotPatrol.GetPathToFollow();
                break;
            
            case State.ReturnState:
                //robotSenses.lockHeadOnTarget = false;
                //robotAlert.ReturnToStatusQuo();
                robotPatrol.ResetToPreviousWaypoint();
                robotPatrol.GetNextWaypoints();
                break;
        }
    }

    Vector3 GetVec3OfPosition(Vector3 ofPosition)
    {
        return new Vector3(ofPosition.x, ofPosition.y, 0f);
    }
}
