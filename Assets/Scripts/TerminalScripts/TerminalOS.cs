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

    public GameObject workingCPUWindow;
    public GameObject connectWindow;
    public GameObject hackboxWindow;

    public IHackable hackableObject;

    public TextMeshProUGUI terminalOutputText;
    public GameController gameController;
    public Image signalFillBar;
    public Image signalFillBarUI;
    public GameObject noSignal;
    public GameObject noSignalUI;
    public TerminalTicker ticker;
    public Button currentCPUButton;
    public TextMeshProUGUI currentCPUText;
    public TextMeshProUGUI currentTime;
    public PlayerController player;

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
        StartCoroutine(TerminalOutput());
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void HackSystem(IHackable _hackableObject)
    {
        if(player.stateMachine.currentState != TerminalState.Instance)
        {
            player.OpenHandTerminal();
        }

        hackableObject = _hackableObject;

        stateMachine.ChangeState(TerminalAccessState.Instance);
        if (hackableObject.GetType() == typeof(Computer))
        {
            Computer computer = hackableObject.GetAttachedGameObject().GetComponent<Computer>();
            Debug.Log("Hacking: " + computer.transform.name);
        }

        if (hackableObject.GetType() == typeof(RobotController))
        {
            RobotController robot = hackableObject.GetAttachedGameObject().GetComponent<RobotController>();
            Debug.Log("Hacking: " + robot.transform.name);
        }
    }

    public void WhattimeIsIt()
    {
        currentTime.text = currentTime.text = System.DateTime.Now.ToString();
    }

    public void ResetOS()
    {

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
        //if(workingCPU == null)
        //{
        //    //terminalAccessIcon.GetComponent<Button>().interactable = false;
        //    //CloseTerminalAccessWindow();
        //    return;
        //}

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
        //if(workingCPU != null)
        //{
        //    float distanceToComputer = MyUtils.DistanceBetweenObjects(player.transform.position, workingCPU.transform.position);
        //    if (workingCPU.accessGranted)
        //    {
        //        currentCPUButton.interactable = true;
        //        currentCPUText.text = "HACKED SYSTEM";
        //    }
        //    else
        //    {
        //        currentCPUButton.interactable = false;
        //        currentCPUText.text = "NO ACCESS";
        //    }
        //
        //    if (distanceToComputer > terminalRange)
        //    {
        //        workingCPU = null;
        //        //workingCPUWindow.SetActive(false);
        //        //workingCPUWindow = null;
        //    }
        //}
        //
        //if(workingCPU == null)
        //{
        //    currentCPUButton.interactable = false;
        //    currentCPUText.text = "NO SYSTEM";
        //}
    }

    public void QueueTerminalMessages(string message)
    {
        string newMessage = message + "\n";
        terminalMessages.Enqueue(newMessage);
    }

    public void ClearQueue()
    {
        terminalMessages.Clear();
        terminalOutputText.text = "      --HACKBOX VERSION 1.8--\n\n";
    }

    IEnumerator TerminalOutput()
    {
        while (true)
        {
            if(terminalMessages.Count > 0)
            {
                string currentMessage = terminalMessages.Peek();
                terminalOutputText.text += ">> ";
                foreach(char c in currentMessage)
                {
                    terminalOutputText.text += c;
                    yield return null;
                }
                terminalMessages.Dequeue();
            }
            yield return null;
        }
    }

    public void LoadTerminalLoadBar(string message, int seconds)
    {
        //StartCoroutine(TerminalLoadBar(message, seconds));
    }

    //IEnumerator TerminalLoadBar(string message, int seconds)
    //{
    //    float timer = seconds / 16f;
    //    string loading = "||||||||||||||||";
    //    terminalText.text += message + ": ";
    //    foreach (char c in loading)
    //    {
    //        terminalText.text += c;
    //        yield return new WaitForSeconds(timer);
    //    }
    //}
}
