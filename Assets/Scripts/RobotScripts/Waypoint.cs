using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Vector2Int GetGridPos()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }
}
