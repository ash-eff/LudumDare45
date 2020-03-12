using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class RobotCircle : MonoBehaviour
{
    public RobotController robot;
    public int segments;
    public float xradius;
    public float yradius;
    PlayerController player;
    public LineRenderer circle;
    public LineRenderer line;
    public GameObject lineRotator;


    void Start()
    {
        robot = GetComponentInParent<RobotController>();
        //circle = gameObject.GetComponent<LineRenderer>();
        player = FindObjectOfType<PlayerController>();
        circle.positionCount = segments + 1;
        line.transform.position = new Vector3(transform.position.x + xradius, transform.position.y, 0f);
        line.useWorldSpace = true;
        circle.useWorldSpace = false;
        CreatePoints();
    }

    private void Update()
    {
        Vector3 direction = player.transform.position - robot.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRotator.transform.rotation = Quaternion.Euler(0, 0, angle);
        line.SetPosition(0, player.transform.position);
        line.SetPosition(1, line.transform.position);
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            circle.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }
}
