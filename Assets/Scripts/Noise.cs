using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    public bool makeNoise;
    public float noiseRadius;
    [Range(0,.25f)]
    public float noiseReductionPercent;
    public LayerMask robotLayer, obstacleLayer;

    private void Update()
    {
        if (makeNoise)
        {
            MakeNoise();
        }
    }

    public void MakeNoise()
    {
        makeNoise = false;
        RaycastHit2D[] nearbyRobots = Physics2D.CircleCastAll(transform.position, noiseRadius, Vector2.right, 0, robotLayer);
        if (nearbyRobots != null)
        {
            foreach (RaycastHit2D robot in nearbyRobots)
            {
                Vector2 directionToRobot = robot.transform.position - transform.position;
                float robotDistFromNoise = directionToRobot.magnitude;
                float noiseVolume = CheckForNoiseRedction(directionToRobot);

                if (robotDistFromNoise < noiseVolume)
                {
                    robot.transform.GetComponent<RobotSenses>().HeardANoise(transform.position);
                }
                else
                {
                    Debug.Log(robot.transform.name + " did not hear this");
                }
            }
        }
    }

    private float CheckForNoiseRedction(Vector2 robotDirection)
    {
        float actualVolume = 0;
        RaycastHit2D[] throughObstacles = Physics2D.RaycastAll(transform.position, robotDirection, noiseRadius, obstacleLayer);
        if(throughObstacles.Length == 0)
        {
            actualVolume = noiseRadius;
        }
        else
        {
            float noiseReducer = throughObstacles.Length * noiseReductionPercent;
            if (noiseReducer > 1)
            {
                actualVolume = 0;
            }
            else
            {
                float reductionAmount = noiseRadius * noiseReducer;
                actualVolume = noiseRadius - reductionAmount;
            }
        }

        return actualVolume;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }
}
