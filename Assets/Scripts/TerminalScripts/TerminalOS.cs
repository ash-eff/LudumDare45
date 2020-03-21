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

    public GameObject workingCPUWindow;
    public GameObject gameSettingsWindow;

    public GameObject connectWindow;
    public GameController gameController;
    public Image signalFillBar;
    public Image signalFillBarUI;
    public GameObject noSignal;
    public GameObject noSignalUI;
    public TerminalTicker ticker;
    public Button currentCPUButton;
    public TextMeshProUGUI currentCPUText;

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
        player.OpenHandTerminal();
    }

    public void ResetOS()
    {
        CloseCurrentCPUWindow();
        CloseGameSettingsWindow();
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
            //terminalAccessIcon.GetComponent<Button>().interactable = false;
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
        if(workingCPU != null)
        {
            if (workingCPU.accessGranted)
            {
                currentCPUButton.interactable = true;
                currentCPUText.text = "HACKED SYSTEM";
            }
            else
            {
                currentCPUButton.interactable = false;
                currentCPUText.text = "NO ACCESS";
            }

            if (workingCPU.DistanceFromPlayer() > terminalRange)
            {
                workingCPU = null;
            }
        }

        if(workingCPU == null)
        {
            currentCPUButton.interactable = false;
            currentCPUText.text = "NO SYSTEM";
        }
    }

    public void OpenCurrentCPUWindow()
    {
        Debug.Log("Open CPU Window");
        workingCPUWindow.SetActive(true);
    }

    public void OpenGameSettingsWindow()
    {
        Debug.Log("Close Settings Window");
        gameSettingsWindow.SetActive(true);
    }

    public void CloseCurrentCPUWindow()
    {
        Debug.Log("Close CPU Window");
        workingCPUWindow.SetActive(false);
    }

    public void CloseGameSettingsWindow()
    {
        Debug.Log("Close Settings Window");
        gameSettingsWindow.SetActive(false);
    }

}
