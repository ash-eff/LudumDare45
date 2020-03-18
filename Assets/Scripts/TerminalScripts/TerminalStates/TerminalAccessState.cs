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

    TerminalOS os;

    public override void EnterState(TerminalOS terminalOS)
    {
        os = terminalOS;
        if (!terminalOS.workingComputer.accessGranted)
        {
            terminalOS.hackGameBar.fillAmount = 0;
            terminalOS.hackGameWindow.SetActive(true);         
        }
    }

    public override void ExitState(TerminalOS terminalOS)
    {
        terminalOS.hackGameWindow.SetActive(false);
    }

    public override void UpdateState(TerminalOS terminalOS)
    {
        if (!RunTempGame())
        {
            terminalOS.stateMachine.ChangeState(TerminalConnectedState.Instance);
        }
    }

    public override void FixedUpdateState(TerminalOS terminalOS)
    {
    }

    bool RunTempGame()
    {
        if(os.hackGameBar.fillAmount < 1)
        {
            os.hackGameBar.fillAmount += Time.deltaTime;
            return true;
        }

        // when you add more hacked icons, you'll need to select the next available one
        os.systemIcons[1].computer = os.workingComputer;
        os.workingComputer.accessGranted = true;
        return false;
    }
}
