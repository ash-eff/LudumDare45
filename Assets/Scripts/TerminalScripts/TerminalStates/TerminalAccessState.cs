using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;

public class TerminalAccessState : State<Terminal>
{
    #region setup
    private static TerminalAccessState _instance;

    private TerminalAccessState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<Terminal> createInstance() { return Instance; }

    public static TerminalAccessState Instance
    {
        get { if (_instance == null) new TerminalAccessState(); return _instance; }
    }
    #endregion

    TerminalOS terminalOS;

    public override void EnterState(Terminal terminal)
    {
        terminalOS = terminal.terminalOS;
        terminalOS.terminalAccessWindow.SetActive(true);
        terminalOS.terminalAccessIcon.SetActive(true);
    }

    public override void ExitState(Terminal terminal)
    {
        terminalOS.terminalAccessWindow.SetActive(false);
        terminalOS.terminalAccessIcon.SetActive(false);
    }

    public override void UpdateState(Terminal terminal)
    {
    }

    public override void FixedUpdateState(Terminal terminal)
    {
    }
}
