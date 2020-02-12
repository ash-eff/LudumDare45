using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    public float maxNoiseRadius;
    public float noiseRadius;
    public float noiseArc;
    [Range(0,.25f)]
    public float noiseReductionPercent;
    public float yOffset;
    public LayerMask robotLayer, obstacleLayer;
    public GameObject indicator;

    private void Start()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, 0f);
        //StartCoroutine(GrowNoise());
        //MakeNoise();
        Debug.Log("Noise");
        Destroy(gameObject, 1f);
    }

    //IEnumerator GrowNoise()
    //{
    //    float lerpTime = .5f;
    //    float currentLerpTime = 0;
    //    while (currentLerpTime < lerpTime)
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        float perc = currentLerpTime / lerpTime;
    //        noiseRadius = Mathf.Lerp(0, maxNoiseRadius, perc);
    //        indicator.transform.localScale = Vector2.one * noiseRadius;
    //        yield return null;
    //    }
    //    Destroy(this.gameObject);
    //}

    //void MakeNoise()
    //{
    //    RaycastHit2D[] nearbyRobots = Physics2D.CircleCastAll(transform.position, noiseRadius, Vector2.right, 0, robotLayer);
    //    if (nearbyRobots.Length > 0)
    //    {
    //        foreach (RaycastHit2D robot in nearbyRobots)
    //        {
    //            //Vector2 directionToRobot = robot.transform.position - transform.position;
    //            //float robotDistFromNoise = directionToRobot.magnitude;
    //            //float noiseVolume = CheckForNoiseRedction(directionToRobot);
    //            //
    //            //if (robotDistFromNoise < noiseVolume)
    //            //{
    //                robot.transform.GetComponent<RobotSenses>().HeardANoise(transform.position);
    //            //}
    //        }
    //    }
    //    //nearbyRobots = null;
    //}

    //IEnumerator MakeNoise(float noiseLength)
    //{
        //float timer = noiseLength;
        //if(noiseLength == 0)
        //{
        //    while (true)
        //    {
                //RaycastHit2D[] nearbyRobots = Physics2D.CircleCastAll(transform.position, noiseRadius, Vector2.right, 0, robotLayer);
                //if (nearbyRobots != null)
                //{
                //    foreach (RaycastHit2D robot in nearbyRobots)
                //    {
                //        Vector2 directionToRobot = robot.transform.position - transform.position;
                //        float robotDistFromNoise = directionToRobot.magnitude;
                //        float noiseVolume = CheckForNoiseRedction(directionToRobot);
                //
                //        if (robotDistFromNoise < noiseVolume)
                //        {
                //            robot.transform.GetComponent<RobotSenses>().HeardANoise(transform.position);
                //        }
                //    }
                //}
                //
                //yield return new WaitForSeconds(.1f);
       //     }
       // }
       // else
       // {
       //     while (timer > 0)
       //     {
       //         RaycastHit2D[] nearbyRobots = Physics2D.CircleCastAll(transform.position, noiseRadius, Vector2.right, 0, robotLayer);
       //         if (nearbyRobots != null)
       //         {
       //             foreach (RaycastHit2D robot in nearbyRobots)
       //             {
       //                 Vector2 directionToRobot = robot.transform.position - transform.position;
       //                 float robotDistFromNoise = directionToRobot.magnitude;
       //                 float noiseVolume = CheckForNoiseRedction(directionToRobot);
       //
       //                 if (robotDistFromNoise < noiseVolume)
       //                 {
       //                     robot.transform.GetComponent<RobotSenses>().HeardANoise(transform.position);
       //                 }
       //                 else
       //                 {
       //                     Debug.Log(robot.transform.name + " did not hear this");
       //                 }
       //             }
       //         }
       //
       //         yield return new WaitForSeconds(.1f);
       //         timer -= .1f;
       //     }
       // }

    //}

    //private float CheckForNoiseRedction(Vector2 robotDirection)
    //{
    //    float actualVolume = 0;
    //    RaycastHit2D[] throughObstacles = Physics2D.RaycastAll(transform.position, robotDirection, noiseRadius, obstacleLayer);
    //    if(throughObstacles.Length == 0)
    //    {
    //        actualVolume = noiseRadius;
    //    }
    //    else
    //    {
    //        float noiseReducer = throughObstacles.Length * noiseReductionPercent;
    //        if (noiseReducer > 1)
    //        {
    //            actualVolume = 0;
    //        }
    //        else
    //        {
    //            float reductionAmount = noiseRadius * noiseReducer;
    //            actualVolume = noiseRadius - reductionAmount;
    //        }
    //    }
    //
    //    return actualVolume;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }
}
