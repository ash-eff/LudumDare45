using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatoe : MonoBehaviour
{
    public float zAngle;

    void Update()
    {
        transform.Rotate(0, 0, zAngle, Space.Self);
    }
}
