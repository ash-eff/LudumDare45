using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShoot : MonoBehaviour
{
    public GameObject robot;
    private PlayerManager playerTarget;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerTarget = FindObjectOfType<PlayerManager>();
    }

    public IEnumerator ShootTarget()
    {
        yield return new WaitForSecondsRealtime(1f);
        lineRenderer.SetPosition(0, robot.transform.position);
        lineRenderer.SetPosition(1, playerTarget.transform.position);
        lineRenderer.enabled = true;
        playerTarget.Kill();
        yield return new WaitForSecondsRealtime(.1f);
        lineRenderer.enabled = false;
    }
}
