using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ash.PlayerController;

public class RobotGUI : MonoBehaviour
{
    public LineRenderer circleRenderer;
    public LineRenderer lineRenderer;
    public GameObject lineRotator;
    public Image downloadCircle;
    public TextMeshProUGUI percentText;
    public Button downloadButton;
    [SerializeField] private GameObject videoSprite;
    [SerializeField] private GameObject videoBackGround;
    [SerializeField] private GameObject exclaim;

    public int circleSegments;
    public float circleRadius;

    public Color inRange;
    public Color outOfRange;

    private PlayerController player;

    private void Awake()
    {
        SetVideoFeedActive(false);
        SetExclaimActive(false);
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        circleRenderer.positionCount = circleSegments + 1;
        lineRenderer.transform.position = new Vector3(transform.position.x + circleRadius, transform.position.y, 0f);
        lineRenderer.useWorldSpace = true;
        circleRenderer.useWorldSpace = false;
        CreatePoints();
        circleRenderer.gameObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
    }

    public void SetVideoFeedActive(bool isActive)
    {
        videoSprite.SetActive(isActive);
        videoBackGround.SetActive(isActive);
    }

    public void SetExclaimActive(bool isActive)
    {
        exclaim.SetActive(isActive);
    }

    public void SetRobotLineRenderersActive(bool isActive)
    {
        circleRenderer.gameObject.SetActive(isActive);
        lineRenderer.gameObject.SetActive(isActive);
    }

    public void SetRobotGUIButtonActive(bool isActive)
    {
        downloadButton.gameObject.SetActive(isActive);
    }

    public void CheckGUILineLength()
    {
        lineRenderer.SetPosition(0, player.circle.transform.position);
        lineRenderer.SetPosition(1, lineRenderer.transform.position);
    }

    public void CheckGUILineRotation()
    {
        Vector3 direction = GetDirectionToTarget(player.circle.transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRotator.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void CheckGUIColor()
    {
        if (CheckRobotGUIRange())
        {
            circleRenderer.startColor = inRange;
            circleRenderer.endColor = inRange;
            lineRenderer.startColor = inRange;
            lineRenderer.endColor = inRange;
        }
        else
        {
            circleRenderer.startColor = outOfRange;
            circleRenderer.endColor = outOfRange;
            lineRenderer.startColor = outOfRange;
            lineRenderer.endColor = outOfRange;
        }
    }

    private bool CheckRobotGUIRange()
    {
        float distance = GetDirectionToTarget(player.circle.transform.position).magnitude;
        if (distance <= 10)
            return true;

        else
            return false;

    }

    private Vector3 GetDirectionToTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        return direction;
    }

    private void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 20f;

        for (int i = 0; i < (circleSegments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * circleRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * circleRadius;

            circleRenderer.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / circleSegments);
        }
    }
}
