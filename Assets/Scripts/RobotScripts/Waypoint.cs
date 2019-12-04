using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Vector3 GetGridPos()
    {
        return new Vector3(transform.position.x, transform.position.y, 0f);
    }
}
