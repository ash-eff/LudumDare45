using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class RobotOS : MonoBehaviour
{
    public RobotController robot;
    public RobotGUI robotGUI;

    public GameObject explosion;
    //public AudioSource audioSource;
    public AudioClip beepBoop;
    public AudioClip beep;
    public AudioClip beepBeepBeep;
    public AudioClip hackOpen;
    public GameObject hackOptions;
    public GameObject videoFeed;

    private bool markRobots;
    private GameController gc; 
    private RobotCam robotCam;
    //private PlayerController player;

    //void OnEnable()
    //{
    //    PlayerController.OnTerminalOpen += TerminalOpened;
    //    PlayerController.OnTerminalClose += TerminalClosed;
    //}
    //
    //
    //void OnDisable()
    //{
    //    PlayerController.OnTerminalOpen -= TerminalOpened;
    //    PlayerController.OnTerminalClose -= TerminalClosed;
    //}

    void Awake()
    {
        gc = FindObjectOfType<GameController>();
    }

    //void TerminalOpened()
    //{
    //    markRobots = true;
    //}

    //void TerminalClosed()
    //{
    //    markRobots = false;
    //}

    //public void RobotClicked()
    //{
    //    StartCoroutine(HackRobot());
    //}
    //
    //IEnumerator HackRobot()
    //{
    //    robotGUI.percentText.gameObject.SetActive(true);
    //    float downloadTime = 2;
    //    audioSource.PlayOneShot(beepBoop);
    //    while (robotGUI.downloadCircle.fillAmount < 1)
    //    {
    //        robotGUI.downloadCircle.fillAmount += (Time.deltaTime / downloadTime);
    //        robotGUI.percentText.text = (robotGUI.downloadCircle.fillAmount * 100).ToString("000");
    //        yield return null;
    //    }
    //
    //    robotCam = FindObjectOfType<RobotCam>();
    //    robotCam.target = this.transform;
    //    robotCam.GetComponent<Camera>().enabled = true;
    //    robot.IsHacked = true;
    //    videoFeed.gameObject.SetActive(true);
    //    //hackOptions.SetActive(true);
    //    //audioSource.Stop();
    //    //audioSource.PlayOneShot(hackOpen);
    //    robotGUI.downloadButton.gameObject.SetActive(false);
    //    robotGUI.percentText.gameObject.SetActive(false);
    //    robotGUI.downloadCircle.fillAmount = 0;
    //
    //}
    //
    //public void HackShutDown()
    //{
    //    Debug.Log("Shut Down");
    //}
    //
    //public void HackBlowUp()
    //{
    //    StartCoroutine(IeBlowUp());
    //}
    //
    //public void HackReverseDirection()
    //{
    //    Debug.Log("Reverse Direction");
    //}
    //
    //public void HackMakeFriendly()
    //{
    //    Debug.Log("Make Friendly");
    //}
    //
    //IEnumerator IeBlowUp()
    //{
    //    hackOptions.SetActive(false);
    //    robotGUI.percentText.text = "3";
    //    robotGUI.percentText.gameObject.SetActive(true);
    //    audioSource.PlayOneShot(beep);
    //    yield return new WaitForSeconds(1f);
    //    robotGUI.percentText.text = "2";
    //    audioSource.PlayOneShot(beep);
    //    yield return new WaitForSeconds(1f);
    //    robotGUI.percentText.text = "1";
    //    audioSource.PlayOneShot(beep);
    //    yield return new WaitForSeconds(1f);
    //    robotGUI.percentText.text = "0";
    //    audioSource.PlayOneShot(beepBeepBeep);
    //    yield return new WaitForSeconds(.5f);
    //    CameraController cam = FindObjectOfType<CameraController>();
    //    cam.CameraShake();
    //    GameObject go = Instantiate(explosion, transform.position, Quaternion.identity);
    //    Destroy(go, 2f);
    //
    //    robot.stateMachine.ChangeState(new RobotExplodeState());
    //}
}
