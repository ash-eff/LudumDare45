using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalLockedState : State<Terminal>
{
    #region setup
    private static TerminalLockedState _instance;

    private TerminalLockedState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<Terminal> createInstance() { return Instance; }

    public static TerminalLockedState Instance
    {
        get { if (_instance == null) new TerminalLockedState(); return _instance; }
    }
    #endregion

    TerminalOS terminalOS;

    public override void EnterState(Terminal terminal)
    {
        terminalOS = terminal.terminalOS;
        terminalOS.AttachTerminal(terminal);
        terminalOS.loadingBarWindow.SetActive(true);
    }

    public override void ExitState(Terminal terminal)
    {
        terminalOS.loadingBar.transform.parent.gameObject.SetActive(false);
    }

    public override void UpdateState(Terminal terminal)
    {
        if (terminalOS.GetLoadAccess(terminal, "Accessing Terminal"))
        {
            terminal.accessGranted = true;
            terminal.stateMachine.ChangeState(TerminalAccessState.Instance);
        }
            
    }

    public override void FixedUpdateState(Terminal terminal)
    {
    }
}
