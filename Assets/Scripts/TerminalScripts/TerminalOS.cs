using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.PlayerController;
using Ash.StateMachine;
using TMPro;

public class TerminalOS : MonoBehaviour
{
    public StateMachine<TerminalOS> stateMachine;
    public static TerminalOS terminalOS;

    public LayerMask objectLayer;
    public float terminalRange;
    public CanvasGroup terminalGUI;
    public CPU workingCPU;
    public TextMeshProUGUI workingCPUName;
    public Image loadingBar;
    public Image securityBar;
    public TextMeshProUGUI loadingText;
    //public GameObject securityGranted;
    public GameObject HackedSystemsWindow;
    public GameObject loadingBarWindow;
    public GameObject securityBarWindow;
    public GameObject terminalAccessWindow;
    public GameObject terminalAccessIcon;
    public GameObject securityAccessIcon;
    public GameObject hackGameWindow;
    public Image hackGameBar;
    public GameController gameController;
    public Image signalFillBar;
    public Image signalFillBarUI;
    public GameObject noSignal;
    public GameObject noSignalUI;
    public TerminalTicker ticker;

    public PlayerController player;

    private Computer[] computers;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        terminalGUI.alpha = 0;
        gameController = FindObjectOfType<GameController>();

        terminalOS = this;
        stateMachine = new StateMachine<TerminalOS>(terminalOS);
        stateMachine.ChangeState(TerminalSleepState.Instance);
    }

    private void Start()
    {
        computers = gameController.currentRoom.computers;
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void SetWorkingCPU(CPU _cpu)
    {
        workingCPU = _cpu;
        workingCPUName.text = workingCPU.transform.name;
        player.HandTerminal();
    }

public void ResetOS()
    {
        loadingBar.fillAmount = 0;
        loadingBar.transform.parent.gameObject.SetActive(false);
        terminalAccessWindow.SetActive(false);
        terminalAccessIcon.SetActive(false);
    }


    IEnumerator FillSecurityBar()
    {
        securityBar.fillAmount = 0;
        securityBarWindow.SetActive(true);
        while (securityBar.fillAmount < 1)
        {
            securityBar.fillAmount += Time.deltaTime;
            yield return null;
        }

        //securityGranted.SetActive(true);
        yield return new WaitForSeconds(.5f);
        //securityGranted.SetActive(false);
        securityBarWindow.SetActive(false);
        PlayerController player = FindObjectOfType<PlayerController>();
        player.TargetRobots(true);
    }


    public void SignalStrength()
    {
        //if(workingComputer == null)
        //{
        //    signalFillBarUI.gameObject.SetActive(false);
        //    signalFillBar.gameObject.SetActive(false);
        //    noSignalUI.SetActive(true);
        //    noSignal.SetActive(true);
        //    return;
        //}
        //
        //signalFillBarUI.gameObject.SetActive(true);
        //signalFillBar.gameObject.SetActive(true);
        //
        //float maxDistance = terminalRange;
        //float fillAmount = 0;
        //float currentDistance = workingComputer.DistanceFromPlayer();
        //
        //if (currentDistance <= terminalRange)
        //{
        //    fillAmount = Mathf.Abs(currentDistance - maxDistance) / terminalRange;
        //    noSignal.SetActive(false);
        //    noSignalUI.SetActive(false);
        //}
        //else
        //{
        //    noSignal.SetActive(true);
        //    noSignalUI.SetActive(true);
        //}
        //
        //
        //signalFillBar.fillAmount = fillAmount;
        //signalFillBarUI.fillAmount = fillAmount;
    }

    public void IsComputerAccessible()
    {
        if(workingCPU == null)
        {
            terminalAccessIcon.GetComponent<Button>().interactable = false;
            //CloseTerminalAccessWindow();
            return;
        }

        //if(workingComputer.DistanceFromPlayer() <= terminalRange)
        //{
        //    terminalAccessIcon.GetComponent<Button>().interactable = true;
        //}
        //else
        //{
        //    terminalAccessIcon.GetComponent<Button>().interactable = false;
        //    CloseTerminalAccessWindow();
        //}
    }

    public void CheckForComputerInRange()
    { 
        //if(computers.Length > 0)
        //{
        //    foreach(Computer computer in computers)
        //    {
        //        if(computer.DistanceFromPlayer() <= terminalRange)
        //        {
        //            computer.PingComputer();
        //            //workingComputer = computer;
        //        }
        //    }
        //}
        //else
        //{
        //    workingComputer = null;
        //}
    }

}
