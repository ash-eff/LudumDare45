using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class RobotSenses : MonoBehaviour
{
    public LayerMask visionLayer;
    //public Transform robotHead;
    public float visionDistance;
    public float baseVisionDistance;
    public float visionAngle;
    public float visionTime;
    public float maxHeadRotation;
    public float scanTime;

    public bool lockHeadOnTarget;
    public bool heardSomething;
    
    public Vector2 locationOfSuspicion;
    public GameObject exclaim;

    public  PlayerController player;
    private RobotController robotController;
    private RobotInvestigate robotInvestigate;
    private GameController gameController;
    
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        robotInvestigate = GetComponent<RobotInvestigate>();
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
        TestingGizmos();
        //if (gameController.IsGameOver)
        //{
        //    return;
        //}
    
        //if(robotController.state == RobotController.State.PatrolState || robotController.state == RobotController.State.ReturnState)
        //{
        //    PatrolScan();
        //}
    
        //if (lockHeadOnTarget)
        //{
        //    float distanceToTarget = ((Vector2)robotHead.transform.position - locationOfSuspicion).magnitude;
        //    float angle = Mathf.Atan2(locationOfSuspicion.y - robotHead.transform.position.y, 
        //                              locationOfSuspicion.x - robotHead.transform.position.x) * Mathf.Rad2Deg;
        //    robotHead.rotation = Quaternion.Euler(0f, 0f, angle);
        //    visionDistance = distanceToTarget;
        //}
        //else
        //{
        //    visionDistance = baseVisionDistance;
        //}
    }
    
    //void PatrolScan()
    //{
    //    robotHead.localRotation = Quaternion.Euler(0f, 0f, maxHeadRotation * Mathf.Sin(Time.time * scanTime));
    //}
    
    public void HeardANoise(Vector3 atPosition)
    {
        if(robotController.state == RobotController.State.PatrolState)
        {
            locationOfSuspicion = atPosition;
            heardSomething = true;
            //Vector2Int noiseLocation = new Vector2Int((int)atPosition.x, (int)atPosition.y);
            robotController.state = RobotController.State.InvestigateState;
            StartCoroutine(Exclaim());
        }
    }

    IEnumerator Exclaim()
    {
        exclaim.SetActive(true);
        while (robotController.state == RobotController.State.InvestigateState)
        {
            yield return null;
        }
        exclaim.SetActive(false);
    }
    
    public IEnumerator Vision()
    {
        while(true)
        {
            Vector2 directionToTarget = player.transform.position - transform.position;
            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

            if (angleToTarget <= visionAngle && directionToTarget.magnitude < visionDistance)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(transform.position, directionToTarget.normalized, visionDistance, visionLayer);
                Debug.DrawRay(transform.position, directionToTarget, Color.cyan);
                if (hit)
                {
                    if (hit.collider.tag == "PlayerVision" || hit.collider.tag == "Player")
                    {
                        if (!player.isStealthed)
                        {
                            Debug.Log("Player Spotted");
                            exclaim.SetActive(true);
                        }
                        else
                        {
                            exclaim.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                exclaim.SetActive(false);
            }

            yield return null;
        }
    }
    //
    //public IEnumerator CenterHead()
    //{
    //    robotHead.localRotation = Quaternion.Euler(0f, 0f, 0f);
    //    float lerpTime = .5f;
    //    float currentLerpTime = 0;
    //    float fromAngle = robotHead.localRotation.z;
    //    while (fromAngle != 0)
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        float perc = currentLerpTime / lerpTime;
    //        float newAngle = Mathf.Lerp(fromAngle, 0, perc);
    //        robotHead.localRotation = Quaternion.Euler(0f, 0f, newAngle);
    //        yield return null;
    //    }
    //
    //    lockHeadOnTarget = true;
    //}
    
    // FOR TESTING ONLY
    void TestingGizmos()
    {
        Debug.DrawRay(transform.position, transform.right * visionDistance, Color.red);
        var leftDirection = Quaternion.AngleAxis(visionAngle, Vector3.forward) * transform.right;
        var rightDirection = Quaternion.AngleAxis(-visionAngle, Vector3.forward) * transform.right;
        Debug.DrawRay(transform.position, new Vector2(rightDirection.x, rightDirection.y) * visionDistance, Color.yellow);
        Debug.DrawRay(transform.position, new Vector2(leftDirection.x, leftDirection.y) * visionDistance, Color.blue);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, visionDistance);
    }
}
