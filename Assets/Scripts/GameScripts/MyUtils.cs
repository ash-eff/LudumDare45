using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUtils
{
    public static Vector2 SnapToGrid(Vector2 position)
    {
        float x = Mathf.Round(position.x * 2) / 2;
        float y = Mathf.Round(position.y * 2) / 2;
        return new Vector2(x, y);
    }
}
