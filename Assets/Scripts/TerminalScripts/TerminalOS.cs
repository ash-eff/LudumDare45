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
    public Computer workingComputer;
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

    public HackedSystemsIcon[] systemIcons;
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

    public void SetWorkingComputer(Computer _computer)
    {
        workingComputer = _computer;
    }

    public void CloseTerminalAccessWindow()
    {
        terminalAccessWindow.SetActive(false);
    }

    public void OpenTerminalAccessWindow()
    {
        terminalAccessWindow.SetActive(true);
    }

    public void ResetOS()
    {
        loadingBar.fillAmount = 0;
        loadingBar.transform.parent.gameObject.SetActive(false);
        terminalAccessWindow.SetActive(false);
        terminalAccessIcon.SetActive(false);
    }

    public void UnlockDoors()
    {
        foreach(Door door in workingComputer.doors)
        {
            if (door.IsLocked)
            {
                door.IsLocked = false;
            }
        }
    }

    public void UseLights()
    {
        workingComputer.UseLights();
    }

    public void SecurityIcon()
    {
        terminalAccessWindow.SetActive(false);
        StartCoroutine(LoadAccess("Accessing Security", securityAccessIcon));
    }

    public void SecuritySystemAccess()
    {
        StartCoroutine(FillSecurityBar());
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

    IEnumerator LoadAccess(string accessText, GameObject icon)
    {
        bool doneLoading = false;
        loadingBar.fillAmount = 0;
        loadingText.text = "";

        while (!doneLoading)
        {
            doneLoading = GetLoadAccess(workingComputer, accessText);

            yield return null;
        }

        icon.SetActive(true);
    }

    public bool GetLoadAccess(Computer _computer, string message)
    {
        loadingBarWindow.SetActive(true);
        loadingText.text = message;
        loadingBar.fillAmount += Time.deltaTime;
        if (loadingBar.fillAmount < 1)
        {
            return false;
        }

        loadingBarWindow.SetActive(false);
        return true;
    }

    public void OpenHackedSystemsWindow()
    {
        HackedSystemsWindow.SetActive(true);
    }

    public void CloseHackedSystemsWindow()
    {
        HackedSystemsWindow.SetActive(false);
    }

    public void SignalStrength()
    {
        if(workingComputer == null)
        {
            signalFillBarUI.gameObject.SetActive(false);
            signalFillBar.gameObject.SetActive(false);
            noSignalUI.SetActive(true);
            noSignal.SetActive(true);
            return;
        }

        signalFillBarUI.gameObject.SetActive(true);
        signalFillBar.gameObject.SetActive(true);

        float maxDistance = terminalRange;
        float fillAmount = 0;
        float currentDistance = workingComputer.DistanceFromPlayer();
        
        if (currentDistance <= terminalRange)
        {
            fillAmount = Mathf.Abs(currentDistance - maxDistance) / terminalRange;
            noSignal.SetActive(false);
            noSignalUI.SetActive(false);
        }
        else
        {
            noSignal.SetActive(true);
            noSignalUI.SetActive(true);
        }

      
        signalFillBar.fillAmount = fillAmount;
        signalFillBarUI.fillAmount = fillAmount;
    }

    public void IsComputerAccessible()
    {
        if(workingComputer == null)
        {
            terminalAccessIcon.GetComponent<Button>().interactable = false;
            CloseTerminalAccessWindow();
            return;
        }

        if(workingComputer.DistanceFromPlayer() <= terminalRange)
        {
            terminalAccessIcon.GetComponent<Button>().interactable = true;
        }
        else
        {
            terminalAccessIcon.GetComponent<Button>().interactable = false;
            CloseTerminalAccessWindow();
        }
    }

    public void CheckForComputerInRange()
    { 
        if(computers.Length > 0)
        {
            foreach(Computer computer in computers)
            {
                if(computer.DistanceFromPlayer() <= terminalRange)
                {
                    computer.PingComputer();
                    //workingComputer = computer;
                }
            }
        }
        else
        {
            workingComputer = null;
        }
    }

}
