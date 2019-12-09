using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInvestigate : MonoBehaviour
{
    public float investigateMoveSpeed;
    
    private RobotController robotController;
    private RobotPatrol robotPatrol;
    private PathFinder pathfinder;
    //private GridMap gridMap;
    
    public Vector3 startPos;
    public Vector3 endPos;
    private Vector3 returnLocation;
    
    public Vector3 GetReturnLocation
    {
        get { return returnLocation; }
    }
    
    private void Start()
    {
        //gridMap = FindObjectOfType<GridMap>();
        //robotController = GetComponent<RobotController>();
        //robotPatrol = GetComponent<RobotPatrol>();
        //pathfinder = GetComponent<PathFinder>();
    }
}
