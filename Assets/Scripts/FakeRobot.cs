using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeRobot : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform waypoint;
    public Camera cam;

    private void Start()
    {
        Debug.Log(waypoint.position);
        agent.SetDestination(waypoint.position);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
 
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
    
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);
                agent.SetDestination(hit.point);
            }
        }
    }
}
