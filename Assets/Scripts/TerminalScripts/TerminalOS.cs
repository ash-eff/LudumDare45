using Ash.PlayerController;
using Ash.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    public TextAsset hackingJargon;
    public TextMeshProUGUI hackingJargonOutputText;

    private string jargon;
    private string[] listOfJargon;
    private Queue<string> jargonOrdered = new Queue<string>();

    public GameObject workingCPUWindow;
    public GameObject connectWindow;
    public GameObject hackboxWindow;
    public GameObject hackingOutputWindow;
    public GameObject hackingProgressWindow;
    public GameObject hackingCompleteWindow;

    public Image antiVirusFill;
    public Image passwordStrengthFill;
    public Image dataEncryptionFill;
    public Image softwareSecurityFill;
    public Image hackingFill;

    public TextMeshProUGUI hackPercentTextDark;
    public TextMeshProUGUI hackPercentTextLight;

    public Hackable hackableObject;

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

    public bool hackActive = false;
    private bool bruteForce;
    private bool trojan;
    private bool rootKit;

    public int bruteforceMod;
    public int trojanMod;
    public int rootkitMod;

    private float antiVirus = 0;
    private float hackTimer = 0;

    private Queue<string> terminalMessages = new Queue<string>();

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        terminalGUI.alpha = 0;
        gameController = FindObjectOfType<GameController>();

        terminalOS = this;
        stateMachine = new StateMachine<TerminalOS>(terminalOS);
        stateMachine.ChangeState(TerminalBaseState.Instance);

        currentTime.text = System.DateTime.Now.ToString();

        jargon = hackingJargon.text;
        listOfJargon = Regex.Split(jargon, "\n");
    }

    private void Start()
    {
        StartCoroutine(TerminalOutput());
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void HackSystem(Hackable _hackableObject)
    {
        if(player.stateMachine.currentState != TerminalState.Instance)
        {
            player.OpenHandTerminal();
        }
        
        if (hackActive)
        {
            return;
        }
        
        hackableObject = _hackableObject;

        stateMachine.ChangeState(TerminalHackState.Instance);
        
        if (hackableObject.GetType() == typeof(Computer))
        {
            Computer computer = hackableObject.GetComponent<Computer>();
        }
        
        if (hackableObject.GetType() == typeof(RobotController))
        {
            RobotController robot = hackableObject.GetComponent<RobotController>();
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
        if(hackableObject == null)
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
        float currentDistance = MyUtils.DistanceBetweenObjects(player.transform.position, hackableObject.transform.position);
        
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

    public void StartHack()
    {
        terminalOS.QueueTerminalMessages("Hack Log Opened...");
        hackingOutputWindow.SetActive(true);
        hackingProgressWindow.SetActive(true);
        hackingJargonOutputText.text = "";
        // set this to be random each time;
        foreach(string s in listOfJargon)
        {
            jargonOrdered.Enqueue(s);
        }

        StartCoroutine(IHackTimer());

    }

    public void LoadStats()
    {
        StartCoroutine(IFillStatBar(passwordStrengthFill, 0f, hackableObject.passwordStrength));
        StartCoroutine(IFillStatBar(dataEncryptionFill, 0f, hackableObject.dataEncryption));
        StartCoroutine(IFillStatBar(softwareSecurityFill, 0f, hackableObject.softwareMaintenance));
        AdjustAntiVirus();
    }

    IEnumerator IHackTimer()
    {
        hackTimer = antiVirus * 2f;
        float fillAmount = 0;
        float maxVal = hackTimer;
        StartCoroutine(IStartHack());
        while (hackTimer > 0)
        {
            hackTimer -= Time.deltaTime;
            fillAmount += Time.deltaTime;
            hackPercentTextDark.text = Mathf.RoundToInt(fillAmount * 10).ToString() + "%";
            hackPercentTextLight.text = Mathf.RoundToInt(fillAmount * 10).ToString() + "%";
            hackingFill.fillAmount = fillAmount / maxVal;
            yield return null;
        }
        hackPercentTextDark.text = "FINALIZING";
        hackPercentTextLight.text = "FINALIZING";
    }

    IEnumerator IStartHack()
    {
        hackingJargonOutputText.text += ">> Beginning Hack. \n";
        yield return null;
        hackingJargonOutputText.text += ">> Opening Ports. \n";
        yield return null;

        while (hackTimer > 0)
        {
            hackingJargonOutputText.text += ">> " + jargonOrdered.Dequeue() + "\n";
            float waitTime = Random.Range(.1f, .5f);
            yield return new WaitForSeconds(waitTime);
        }

        hackingJargonOutputText.text += ">> Hack Complete. \n";
        terminalOS.QueueTerminalMessages("Access Granted...");
        yield return new WaitForSeconds(.5f);

        // swap terminal state and shut off all windows needed
        stateMachine.ChangeState(TerminalSysAccessState.Instance);
    }

    public void HackingCompleteClose()
    {
        terminalOS.QueueTerminalMessages("Acessing Remote System Window...");
        hackingCompleteWindow.SetActive(false);
        workingCPUWindow.SetActive(true);
    }

    public void BruteForce()
    {
        bruteForce = !bruteForce;
        float currentVal = passwordStrengthFill.fillAmount;
        if (bruteForce)
        {
            terminalOS.QueueTerminalMessages("Loading Brute Force Tool...");
            hackableObject.passwordStrength -= bruteforceMod;
        }

        else
        {
            terminalOS.QueueTerminalMessages("Brute Force Tool Disabled...");
            hackableObject.passwordStrength += bruteforceMod;
        }

        if (hackableObject.passwordStrength < 0)
            hackableObject.passwordStrength = 0;
        if (hackableObject.passwordStrength > 5)
            hackableObject.passwordStrength = 5;

        StartCoroutine(IFillStatBar(passwordStrengthFill, currentVal, hackableObject.passwordStrength));
        AdjustAntiVirus();
    }

    public void Trojan()
    {
        trojan = !trojan;
        float currentVal = dataEncryptionFill.fillAmount;
        if (trojan)
        {
            terminalOS.QueueTerminalMessages("Loading Trojan...");
            hackableObject.dataEncryption -= trojanMod;
        }

        else
        {
            terminalOS.QueueTerminalMessages("Trojan Disabled...");
            hackableObject.dataEncryption += trojanMod;
        }

        if (hackableObject.dataEncryption < 0)
            hackableObject.dataEncryption = 0;
        if (hackableObject.dataEncryption > 5)
            hackableObject.dataEncryption = 5;

        StartCoroutine(IFillStatBar(dataEncryptionFill, currentVal, hackableObject.dataEncryption));
        AdjustAntiVirus();
    }

    public void RootKit()
    {
        rootKit = !rootKit;
        float currentVal = softwareSecurityFill.fillAmount;
        if (rootKit)
        {
            terminalOS.QueueTerminalMessages("Loading Rootkit...");
            hackableObject.softwareMaintenance -= rootkitMod;
        }

        else
        {
            terminalOS.QueueTerminalMessages("Rootkit Disabled...");
            hackableObject.softwareMaintenance += rootkitMod;
        }

        if (hackableObject.softwareMaintenance < 0)
            hackableObject.softwareMaintenance = 0;
        if (hackableObject.softwareMaintenance > 5)
            hackableObject.softwareMaintenance = 5;

        StartCoroutine(IFillStatBar(softwareSecurityFill, currentVal, hackableObject.softwareMaintenance));
        AdjustAntiVirus();
    }

    void AdjustAntiVirus()
    {
        float currentVal = antiVirusFill.fillAmount;
        antiVirus = (hackableObject.passwordStrength + hackableObject.dataEncryption + hackableObject.softwareMaintenance) / 3f;
        Debug.Log(antiVirus);
        StartCoroutine(IFillStatBar(antiVirusFill, currentVal, antiVirus));
    }

    IEnumerator IFillStatBar(Image imgToFill, float fromVal, float fillVal)
    {
        float lerpTime = .5f;
        float currentLerpTime = 0;
        float fillAmount = fillVal / 5f;
        while(currentLerpTime < lerpTime)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            imgToFill.fillAmount = Mathf.Lerp(fromVal, fillAmount, perc);
            yield return null;
        }

        imgToFill.fillAmount = fillAmount;
    }

    public void CloseCurrentCPUWindow()
    {
        workingCPUWindow.SetActive(false);
    }
}
