using Ash.PlayerController;
using Ash.StateMachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalOS : MonoBehaviour
{
    public StateMachine<TerminalOS> stateMachine;
    public static TerminalOS terminalOS;

    public LayerMask objectLayer;
    public float terminalRange;
    public CanvasGroup terminalGUI;
    public CPU workingCPU;

    public GameObject workingCPUPosition;
    public GameObject workingCPUWindow;
    public GameObject storeWindow;
    public GameObject settingsWindow;
    public GameObject messageWindow;
    public GameObject collectiblesWindow;
    public GameObject gamesWindow;
    public GameObject weeb;

    public TextMeshProUGUI terminalText;

    public GameObject connectWindow;
    public GameController gameController;
    public Image signalFillBar;
    public Image signalFillBarUI;
    public GameObject noSignal;
    public GameObject noSignalUI;
    public TerminalTicker ticker;
    public Button currentCPUButton;
    public TextMeshProUGUI currentCPUText;
    public TextMeshProUGUI currentTime;
    //public static System.DateTime now;

    public PlayerController player;

    private Computer[] computers;

    private Queue<string> terminalMessages = new Queue<string>();

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        terminalGUI.alpha = 0;
        gameController = FindObjectOfType<GameController>();

        terminalOS = this;
        stateMachine = new StateMachine<TerminalOS>(terminalOS);
        stateMachine.ChangeState(TerminalSleepState.Instance);

        currentTime.text = System.DateTime.Now.ToString();
    }

    private void Start()
    {
        computers = gameController.currentRoom.computers;
        StartCoroutine(TerminalOutput());
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void WhattimeIsIt()
    {
        currentTime.text = currentTime.text = System.DateTime.Now.ToString();
    }

    public void SetWorkingCPU(CPU _cpu)
    {
        workingCPU = _cpu;
        workingCPUWindow = _cpu.cpuWindow;
        workingCPUWindow.transform.parent = workingCPUPosition.transform;
        workingCPUWindow.transform.localPosition = Vector3.zero;
        workingCPUWindow.transform.localScale = Vector3.one;
        workingCPUWindow.SetActive(true);
        workingCPUWindow.transform.SetAsFirstSibling();
        player.OpenHandTerminal();
    }

    public void ResetOS()
    {
        CloseCurrentCPUWindow();
        CloseSettingsWindow();
        CloseMessageWindow();
        CloseCollectiblesWindow();
        CloseStoreWindow();
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
                workingCPUWindow.transform.parent = workingCPU.canvas.transform;
                workingCPUWindow.SetActive(false);
                workingCPUWindow = null;
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
        workingCPUPosition.SetActive(true);
        QueueTerminalMessages("Remote Inteface");
        QueueTerminalMessages("...");
    }

    public void OpenStoreWindow()
    {
        storeWindow.SetActive(true);
    }

    public void CloseCurrentCPUWindow()
    {
        workingCPUPosition.SetActive(false);
    }

    public void CloseStoreWindow()
    {
        storeWindow.SetActive(false);
    }

    public void OpenSettingsWindow()
    {
        settingsWindow.SetActive(true);
        Time.timeScale = 0;
        QueueTerminalMessages("Loading Settings");
        QueueTerminalMessages("Game Paused");
        QueueTerminalMessages("...");
    }

    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenMessageWindow()
    {
        QueueTerminalMessages("Loading Messages");
        QueueTerminalMessages("...");
        messageWindow.SetActive(true);
    }

    public void CloseMessageWindow()
    {
        messageWindow.SetActive(false);
    }

    public void OpenCollectiblesWindow()
    {
        collectiblesWindow.SetActive(true);
    }

    public void CloseCollectiblesWindow()
    {
        collectiblesWindow.SetActive(false);
    }

    public void OpenGamesWindow()
    {
        QueueTerminalMessages("Loading Games");
        QueueTerminalMessages("...");
        gamesWindow.SetActive(true);
    }

    public void CloseGamesWindow()
    {
        gamesWindow.SetActive(false);
    }

    public void QueueTerminalMessages(string message)
    {
        string newMessage = message + "\n";
        terminalMessages.Enqueue(newMessage);
    }

    public void ClearQueue()
    {
        terminalMessages.Clear();
        terminalText.text = "      --HACKBOX VERSION 1.8--\n\n";
    }

    IEnumerator TerminalOutput()
    {
        while (true)
        {
            if(terminalMessages.Count > 0)
            {
                string currentMessage = terminalMessages.Peek();
                terminalText.text += ">> ";
                foreach(char c in currentMessage)
                {
                    terminalText.text += c;
                    yield return null;
                }
                terminalMessages.Dequeue();
            }
            else
            {
                Debug.Log("Terminal Queue is empty");
            }

            yield return null;
        }
    }
}
