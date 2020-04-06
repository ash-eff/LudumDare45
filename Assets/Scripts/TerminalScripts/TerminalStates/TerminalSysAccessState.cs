using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalSysAccessState : State<TerminalOS>
{
    #region setup

    private static TerminalSysAccessState _instance;

    private TerminalSysAccessState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<TerminalOS> createInstance() { return Instance; }

    public static TerminalSysAccessState Instance
    {
        get { if (_instance == null) new TerminalSysAccessState(); return _instance; }
    }
    #endregion

    public override void EnterState(TerminalOS terminalOS)
    {
        terminalOS.hackboxWindow.SetActive(false);
        terminalOS.hackingOutputWindow.SetActive(false);
        terminalOS.hackingProgressWindow.SetActive(false);
        terminalOS.hackingCompleteWindow.SetActive(true);
        terminalOS.hackActive = false;
    }

    public override void ExitState(TerminalOS terminalOS)
    {
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
}
