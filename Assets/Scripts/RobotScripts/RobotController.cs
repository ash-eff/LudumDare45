using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public enum State { PatrolState, InvestigateState, AlertState, WaitState, BreakState }
    public State state;

    private PathFinder pathfinder;
    private RobotPathing robotPathing;

    private void Start()
    {
        robotPathing = GetComponent<RobotPathing>();
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
        robotPathing.StartPathing();
    }
}
