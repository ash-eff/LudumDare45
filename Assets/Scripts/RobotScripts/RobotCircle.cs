﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.PlayerController;
using TMPro;

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
    public TextMeshProUGUI percentText;
    public Button downloadButton;
    public GameObject explosion;
    public AudioSource audioSource;
    public AudioClip beepBoop;
    public AudioClip beep;
    public AudioClip beepBeepBeep;
    public AudioClip hackOpen;
    public GameObject hackOptions;
    public GameObject videoFeed;
    bool markRobots;
    GameController gc;

    RobotCam robotCam;

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
                if (!robot.isHacked)
                {
                    downloadButton.gameObject.SetActive(true);
                }
                else
                {
                    downloadButton.gameObject.SetActive(false);
                }
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
            percentText.gameObject.SetActive(false);
            //hackOptions.SetActive(false);
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
        percentText.gameObject.SetActive(true);
        float downloadTime = 2;
        audioSource.PlayOneShot(beepBoop);
        while(downloadCircle.fillAmount < 1)
        {
            downloadCircle.fillAmount += (Time.deltaTime / downloadTime);
            percentText.text = (downloadCircle.fillAmount * 100).ToString("000");
            yield return null;
        }

        robotCam = FindObjectOfType<RobotCam>();
        robotCam.target = this.transform;
        robotCam.GetComponent<Camera>().enabled = true;
        robot.isHacked = true;
        videoFeed.gameObject.SetActive(true);
        //hackOptions.SetActive(true);
        //audioSource.Stop();
        //audioSource.PlayOneShot(hackOpen);
        downloadButton.gameObject.SetActive(false);
        percentText.gameObject.SetActive(false);
        downloadCircle.fillAmount = 0;

    }

    public void HackShutDown()
    {
        Debug.Log("Shut Down");
    }

    public void HackBlowUp()
    {
        StartCoroutine(IeBlowUp());
    }

    public void HackReverseDirection()
    {
        Debug.Log("Reverse Direction");
    }

    public void HackMakeFriendly()
    {
        Debug.Log("Make Friendly");
    }

    IEnumerator IeBlowUp()
    {
        hackOptions.SetActive(false);
        percentText.text = "3";
        percentText.gameObject.SetActive(true);
        audioSource.PlayOneShot(beep);
        yield return new WaitForSeconds(1f);
        percentText.text = "2";
        audioSource.PlayOneShot(beep);
        yield return new WaitForSeconds(1f);
        percentText.text = "1";
        audioSource.PlayOneShot(beep);
        yield return new WaitForSeconds(1f);
        percentText.text = "0";
        audioSource.PlayOneShot(beepBeepBeep);
        yield return new WaitForSeconds(.5f);
        GameObject go = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(go, 2f);

        robot.stateMachine.ChangeState(new RobotExplodeState());
    }
}
