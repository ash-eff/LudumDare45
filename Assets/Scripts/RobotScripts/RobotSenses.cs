using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSenses : MonoBehaviour
{
    public LayerMask visionLayer, itemLayer;
    public Transform robotHead;

    public float visionDistance;
    public float baseVisionDistance;
    public float visionAngle;
    public float visionTime;
    public float maxHeadRotation;
    public float scanTime;
    public bool lockHeadOnTarget;

    public bool heardSomething;

    public Vector2 locationOfSuspicion;

    private RobotController robotController;
    private GameController gameController;

    private void Awake()
    {
        robotController = GetComponent<RobotController>();
        gameController = FindObjectOfType<GameController>();
        baseVisionDistance = visionDistance;
    }

    private void Start()
    {
        StartCoroutine(Vision());
    }

    void Update()
    {
        if (gameController.IsGameOver)
        {
            return;
        }

        if(robotController.state == RobotController.State.PatrolState)
        {
            PatrolScan();
        }

        if (lockHeadOnTarget)
        {
            float distanceToTarget = ((Vector2)robotHead.transform.position - locationOfSuspicion).magnitude;
            float angle = Mathf.Atan2(locationOfSuspicion.y - robotHead.transform.position.y, 
                                      locationOfSuspicion.x - robotHead.transform.position.x) * Mathf.Rad2Deg;
            robotHead.rotation = Quaternion.Euler(0f, 0f, angle);
            visionDistance = distanceToTarget;
        }
        else
        {
            visionDistance = baseVisionDistance;
        }
    }

    void PatrolScan()
    {
        robotHead.localRotation = Quaternion.Euler(0f, 0f, maxHeadRotation * Mathf.Sin(Time.time * scanTime));
    }

    public void HeardANoise(Vector2 atPosition)
    {
        if (!heardSomething)
        {
            locationOfSuspicion = atPosition;
            heardSomething = true;
            Vector2Int noiseLocation = new Vector2Int((int)atPosition.x, (int)atPosition.y);
            //Debug.DrawRay(transform.position, (Vector3)atPosition - transform.position, Color.yellow);
            robotController.state = RobotController.State.AlertState;
        }
    }

    IEnumerator Vision()
    {
        while(robotController.state == RobotController.State.PatrolState)
        {
            RaycastHit2D[] visableTargets = Physics2D.CircleCastAll(transform.position, visionDistance, transform.right, 0, itemLayer);
            foreach(RaycastHit2D visableTarget in visableTargets)
            {
                Vector2 targetPos = visableTarget.transform.position;
                Vector2 directionToTarget = targetPos - (Vector2)robotHead.position;
                float angleToTarget = Vector2.Angle(directionToTarget, robotHead.right);

                RaycastHit2D hit;
                hit = Physics2D.Raycast(robotHead.position, directionToTarget.normalized, visionDistance, visionLayer);
                if (hit.transform.tag == "Player")
                {
                    if (angleToTarget <= visionAngle)
                    {
                        Debug.DrawRay(transform.position, directionToTarget.normalized * directionToTarget.magnitude, Color.cyan);
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, directionToTarget.normalized * directionToTarget.magnitude, Color.magenta);
                    }
                }
            }
            yield return new WaitForSeconds(visionTime);
        }
    }

    public IEnumerator CenterHead()
    {
        robotHead.localRotation = Quaternion.Euler(0f, 0f, 0f);
        float lerpTime = .5f;
        float currentLerpTime = 0;
        float fromAngle = robotHead.localRotation.z;
        while (fromAngle != 0)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            float newAngle = Mathf.Lerp(fromAngle, 0, perc);
            robotHead.localRotation = Quaternion.Euler(0f, 0f, newAngle);
            yield return null;
        }

        lockHeadOnTarget = true;
    }

    // FOR TESTING ONLY
    //void TestingGizmos()
    //{
    //    Debug.DrawRay(robotHead.position, robotHead.right * visionDistance, Color.red);
    //    var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * robotHead.right;
    //    var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * robotHead.right;
    //    Debug.DrawRay(robotHead.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
    //    Debug.DrawRay(robotHead.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, visionDistance);
    //}
}
