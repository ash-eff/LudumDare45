using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUtils
{
    public static Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x * 2) / 2;
        float y = Mathf.Round(position.y * 2) / 2;
        float z = Mathf.Round(position.z * 2) / 2;
        return new Vector3(x, y, z);
    }
}
