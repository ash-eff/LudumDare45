using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;

public class TerminalSleepState : State<Terminal>
{
    #region setup
    private static TerminalSleepState _instance;

    private TerminalSleepState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<Terminal> createInstance() { return Instance; }

    public static TerminalSleepState Instance
    {
        get { if (_instance == null) new TerminalSleepState(); return _instance; }
    }
    #endregion

    public override void EnterState(Terminal terminal)
    {
    }

    public override void ExitState(Terminal terminal)
    {
    }

    public override void UpdateState(Terminal terminal)
    {
    }

    public override void FixedUpdateState(Terminal terminal)
    {
    }
}
