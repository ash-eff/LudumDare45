using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeRobot : MonoBehaviour
{
    public RobotFOV fov;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public GameObject spriteObj;
    public Animator anim;
    public int newIndex;

    private void Start()
    {
        StartCoroutine(PatrolPath());
    }

    private void Update()
    {
        FacingDirection();
    }

    void FacingDirection()
    {
        float xSign = 0;
        float ySign = 0;
        
        if (agent.velocity.x > 1 || agent.velocity.x < -1)
        {
            xSign = Mathf.Sign(agent.velocity.x);
        }

        if(agent.velocity.z > 1 || agent.velocity.z < -1)
        {
            ySign = Mathf.Sign(agent.velocity.z);
        }

        if(xSign == 0 && ySign == 0)
        {
            fov.transform.right = -Vector2.up;
        }
        else
        {
            fov.transform.right = new Vector2(xSign, ySign);
        }

        Vector2 dir2D = new Vector3(xSign, ySign);
        
        anim.SetFloat("DirX", dir2D.x);
        anim.SetFloat("DirY", dir2D.y);

        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    bool CheckForPathCompletion()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    int SelectWaypointIndex(int currentIndex)
    {
        newIndex = Random.Range(0, waypoints.Length);

        if(currentIndex == newIndex)
        {
            newIndex++;
        }

        if(newIndex >= waypoints.Length)
        {
            newIndex = 0;
        }

        return newIndex;
    }

    IEnumerator PatrolPath()
    {
        int currentIndex = 0;
        agent.SetDestination(waypoints[currentIndex].position);
        while (true)
        {
            if (CheckForPathCompletion())
            {
                currentIndex = SelectWaypointIndex(currentIndex);
                agent.SetDestination(waypoints[currentIndex].position);
            }

            yield return null;
        }
    }
}
