using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShoot : MonoBehaviour
{
    public GameObject robot;
    private PlayerManager playerTarget;
    private LineRenderer lineRenderer;
    public AudioSource audioSource;
    public AudioClip robotShoot;
    private MenuController menuController;

    private void Awake()
    {
        menuController = FindObjectOfType<MenuController>();
        lineRenderer = GetComponent<LineRenderer>();
        playerTarget = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        audioSource.volume = menuController.SFXVolume;
    }

    public IEnumerator ShootTarget()
    {
        yield return new WaitForSecondsRealtime(1f);
        lineRenderer.SetPosition(0, robot.transform.position);
        lineRenderer.SetPosition(1, playerTarget.transform.position);
        lineRenderer.enabled = true;
        audioSource.PlayOneShot(robotShoot);
        playerTarget.Kill();
        yield return new WaitForSecondsRealtime(.1f);
        lineRenderer.enabled = false;
    }
}
