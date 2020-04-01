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

    public override void EnterState(TerminalOS terminalOS)
    {
        terminalOS.hackboxWindow.SetActive(true);
        terminalOS.QueueTerminalMessages("Hackbox Launch Panel Ready...");
    }

    public override void ExitState(TerminalOS terminalOS)
    {
    }

    public override void UpdateState(TerminalOS terminalOS)
    {
        terminalOS.SignalStrength();
        terminalOS.IsComputerAccessible();
        terminalOS.CheckForComputerInRange();
        terminalOS.WhattimeIsIt();
    }

    public override void FixedUpdateState(TerminalOS terminalOS)
    {
    }
}
