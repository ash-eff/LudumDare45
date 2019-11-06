using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInvestigate : MonoBehaviour
{
    public float investigateMoveSpeed;

    private RobotController robotController;
    private RobotPatrol robotPatrol;
    private PathFinder pathfinder;

    private Vector2Int startPos;
    private Vector2Int endPos;
    private Vector2 returnLocation;

    public Vector2Int GetReturnLocation
    {
        get { return new Vector2Int((int)returnLocation.x, (int)returnLocation.y); }
    }

    private void Start()
    {
        robotController = GetComponent<RobotController>();
        robotPatrol = GetComponent<RobotPatrol>();
        pathfinder = GetComponent<PathFinder>();
    }

    public IEnumerator StartInvestigation(Vector2Int _start, Vector2Int _end)
    {
        startPos = _start;
        endPos = _end;
        yield return new WaitForSeconds(1f);
        GetPathToInvestigate();
    }

    public void GetPathToInvestigate()
    {
        var path = pathfinder.GetPath(startPos, endPos);
        if (path != null)
        {
            StartCoroutine(InvestigatePath(path));
        }
    }

    IEnumerator InvestigatePath(List<Vector2Int> path)
    {
        int stateValue = (int)robotController.state;
        int nextIndexInPath = 0;
        Vector3 lastPosInList = new Vector3(path[path.Count - 1].x, path[path.Count - 1].y, 0f);

        foreach (Vector2Int vec in path)
        {
            nextIndexInPath++; ;

            if (nextIndexInPath > path.Count - 1)
            {
                // this is the last vec in path
            }
            else
            {
                StartCoroutine(robotPatrol.RotateTowardsTarget(new Vector2(path[nextIndexInPath].x, path[nextIndexInPath].y)));
            }

            while (transform.position != new Vector3(vec.x, vec.y, 0))
            {
                transform.position = Vector2.MoveTowards(transform.position, vec, investigateMoveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        StartCoroutine(FinishInvestigation());
    }

    IEnumerator FinishInvestigation()
    {
        Debug.Log("Finish Investigation");
        yield return new WaitForSeconds(1f);

        // clear are on move on
        // the choice on how to deal with the investigation will branch here
        // for now we will just clean up the item and move on
        robotController.ItemToInvestigate.GetComponent<ValuableItem>().CheckIfItemIsOutOfPosition();
        returnLocation = robotController.ItemToInvestigate.GetComponent<ValuableItem>().originalItemLocation;
        robotController.state = RobotController.State.ReturnState;
        robotController.React();
    }
}
