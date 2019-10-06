using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShoot : MonoBehaviour
{
    public GameObject robot;
    private PlayerManager playerTarget;
    private LineRenderer lineRenderer;

    private void Start()
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
        yield return new WaitForSecondsRealtime(.25f);
        lineRenderer.enabled = false;
        playerTarget.Kill();
    }
}
