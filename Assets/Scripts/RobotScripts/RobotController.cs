using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public enum State { PatrolState, SearchState, InvestigateState, ReturnState, AlarmState, WaitState, BreakState }
    public State state;

    private PathFinder pathfinder;
    private RobotPatrol robotPatrol;
    private RobotSenses robotSenses;
    //private RobotAlert robotAlert;
    //private RobotInvestigate robotInvestigate;

    private GameObject itemOfInvestigation;

    public GameObject ItemToInvestigate
    {
        get { return itemOfInvestigation; }
        set { itemOfInvestigation = value; }
    }

    private void Start()
    {
        //robotInvestigate = GetComponent<RobotInvestigate>();
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
        robotPatrol.GetNextWaypoints(transform.position);
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
                robotPatrol.GetNextWaypoints(transform.position);
                robotPatrol.GetPathToFollow();
                break;

            //case State.InvestigateState:
            //    robotPatrol.ResetToPreviousWaypoint();
            //    Vector2 investigatePosition = robotSenses.locationOfSuspicion;
            //    StartCoroutine(robotPatrol.RotateTowardsTarget(investigatePosition));
            //    robotAlert.OnAlert();
            //    StartCoroutine(robotInvestigate.StartInvestigation(GetVec3OfPosition(transform.position), GetVec3OfPosition(investigatePosition)));
            //    break;
            //
            //case State.ReturnState:
            //    robotSenses.lockHeadOnTarget = false;
            //    robotAlert.ReturnToStatusQuo();
            //    robotPatrol.SetPathStartAndEnd(GetVec3OfPosition(transform.position), robotInvestigate.GetReturnLocation);
            //    robotPatrol.GetPathToFollow();
            //    break;
        }
    }

    Vector3 GetVec3OfPosition(Vector3 ofPosition)
    {
        return new Vector3(ofPosition.x, ofPosition.y, 0f);
    }
}
