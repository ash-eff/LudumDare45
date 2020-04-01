using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;

public class TerminalAccessState : State<TerminalOS>
{
    #region setup
    private static TerminalAccessState _instance;

    private TerminalAccessState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<TerminalOS> createInstance() { return Instance; }

    public static TerminalAccessState Instance
    {
        get { if (_instance == null) new TerminalAccessState(); return _instance; }
    }
    #endregion

    float timer = 0;
    float connectTime = 2f;

    public override void EnterState(TerminalOS terminalOS)
    {
        if (!terminalOS.hackableObject.AccessGranted())
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
        terminalOS.currentCPUText.text = terminalOS.hackableObject.GetSystemName();
    }

    public override void UpdateState(TerminalOS terminalOS)
    {
        if (!RunTempGame())
        {
            terminalOS.connectWindow.SetActive(false);
            terminalOS.stateMachine.ChangeState(TerminalHackState.Instance);
        }
        terminalOS.CheckForComputerInRange();
        terminalOS.WhattimeIsIt();
    }

    public override void FixedUpdateState(TerminalOS terminalOS)
    {
    }

    bool RunTempGame()
    {
        if(timer < connectTime)
        {
            timer += Time.deltaTime;
            return true;
        }

        // when you add more hacked icons, you'll need to select the next available one
        //os.systemIcons[1].computer = os.workingInterface;
        //os.workingCPU.accessGranted = true;
        return false;
    }
}
