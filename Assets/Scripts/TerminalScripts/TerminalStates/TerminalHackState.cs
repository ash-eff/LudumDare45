using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalHackState : State<TerminalOS>
{
    #region setup
    private static TerminalHackState _instance;

    private TerminalHackState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<TerminalOS> createInstance() { return Instance; }

    public static TerminalHackState Instance
    {
        get { if (_instance == null) new TerminalHackState(); return _instance; }
    }
    #endregion

    float timer = 0;
    float connectTime = 2f;
    TerminalOS _terminalOS;

    public override void EnterState(TerminalOS terminalOS)
    {
        _terminalOS = terminalOS;
        SimpleCoroutine.Instance.StartCoroutine(DisplayConnectingWindow());
        //terminalOS.hackboxWindow.SetActive(true);
        //terminalOS.QueueTerminalMessages("Hackbox Launch Panel Ready...");
        terminalOS.hackActive = true;

        if (!terminalOS.hackableObject.AccessGranted)
        {
            timer = 0f;
            terminalOS.connectWindow.SetActive(true);
            terminalOS.currentCPUText.text = "CONNECTING...";
            terminalOS.QueueTerminalMessages("Accessing remote interface");
            terminalOS.QueueTerminalMessages("Please wait...");
        }
    }

    public override void ExitState(TerminalOS terminalOS)
    {
        terminalOS.connectWindow.SetActive(false);
        terminalOS.currentCPUText.text = terminalOS.hackableObject.name;
    }

    public override void UpdateState(TerminalOS terminalOS)
    {
        terminalOS.SignalStrength();
        terminalOS.CheckForComputerInRange();
        terminalOS.WhattimeIsIt();
    }

    public override void FixedUpdateState(TerminalOS terminalOS)
    {
    }

    IEnumerator DisplayConnectingWindow()
    {
        while(timer < connectTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _terminalOS.connectWindow.SetActive(false);
        _terminalOS.hackboxWindow.SetActive(true);
        _terminalOS.LoadStats();
    }

    //bool RunTempGame()
    //{
    //    if (timer < connectTime)
    //    {
    //        timer += Time.deltaTime;
    //        return true;
    //    }
    //
    //    // when you add more hacked icons, you'll need to select the next available one
    //    //os.systemIcons[1].computer = os.workingInterface;
    //    //os.workingCPU.accessGranted = true;
    //    return false;
    //}
}
