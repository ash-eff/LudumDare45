using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalBaseState : State<TerminalOS>
{
    #region setup
    private static TerminalBaseState _instance;

    private TerminalBaseState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<TerminalOS> createInstance() { return Instance; }

    public static TerminalBaseState Instance
    {
        get { if (_instance == null) new TerminalBaseState(); return _instance; }
    }
    #endregion

    public override void EnterState(TerminalOS terminalOS)
    {
    }

    public override void ExitState(TerminalOS terminalOS)
    {
    }

    public override void UpdateState(TerminalOS terminalOS)
    {
        terminalOS.SignalStrength();
        terminalOS.IsComputerAccessible();
    }

    public override void FixedUpdateState(TerminalOS terminalOS)
    {
    }
}
