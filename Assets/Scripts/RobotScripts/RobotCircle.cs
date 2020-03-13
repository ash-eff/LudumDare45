using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Image downloadCircle;
    public Button downloadButton;
    bool markRobots;
    GameController gc;

    public Color inRange;
    public Color outOfRange;

    void OnEnable()
    {
        PlayerController.OnTerminalOpen += TerminalOpened;
        PlayerController.OnTerminalClose += TerminalClosed;
    }


    void OnDisable()
    {
        PlayerController.OnTerminalOpen -= TerminalOpened;
        PlayerController.OnTerminalClose -= TerminalClosed;
    }

    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
    }

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
        circle.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (markRobots && robot.startingRoom == gc.currentRoom)
        {
            Vector3 direction = player.circle.transform.position - robot.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            lineRotator.transform.rotation = Quaternion.Euler(0, 0, angle);
            line.SetPosition(0, player.circle.transform.position);
            line.SetPosition(1, line.transform.position);
            circle.gameObject.SetActive(true);
            line.gameObject.SetActive(true);
            float distance = direction.magnitude;
            if(distance <= 10)
            {
                downloadButton.gameObject.SetActive(true);
                circle.startColor = inRange;
                circle.endColor = inRange;
                line.startColor = inRange;
                line.endColor = inRange;
            }
            else
            {
                downloadButton.gameObject.SetActive(false);
                circle.startColor = outOfRange;
                circle.endColor = outOfRange;
                line.startColor = outOfRange;
                line.endColor = outOfRange;
            }
        }
        else
        {
            downloadButton.gameObject.SetActive(false);
            circle.gameObject.SetActive(false);
            line.gameObject.SetActive(false);
        }
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

    void TerminalOpened()
    {
        markRobots = true;
    }

    void TerminalClosed()
    {
        markRobots = false;
    }

    public void RobotClicked()
    {
        StartCoroutine(HackRobot());
    }

    IEnumerator HackRobot()
    {
        float downloadTime = 2;
        while(downloadCircle.fillAmount < 1)
        {
            downloadCircle.fillAmount += (Time.deltaTime / downloadTime);
            yield return null;
        }
    }
}
