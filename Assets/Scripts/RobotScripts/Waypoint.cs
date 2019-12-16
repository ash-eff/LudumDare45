using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Color baseColor;
    public Color highlightColor;
    private SpriteRenderer spr;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    public Vector3 GetGridPos()
    {
        return new Vector3(transform.position.x, transform.position.y, 0f);
    }

    public void SetHighlightColor()
    {
        spr.color = highlightColor;
    }

    public void ResetToBaseColor()
    {
        spr.color = baseColor;
    }
}
