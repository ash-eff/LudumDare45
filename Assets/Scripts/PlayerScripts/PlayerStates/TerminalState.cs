using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class TerminalState : State<PlayerController>
{
    #region setup
    private static TerminalState _instance;

    private TerminalState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static TerminalState Instance
    {
        get { if (_instance == null) new TerminalState(); return _instance; }
    }
    #endregion

    CanvasGroup terminalGUI;

    public override void EnterState(PlayerController player)
    {
        terminalGUI = player.terminalGUI;
        //player.interactText.text = "Press E to close";
        OpenTerminal();
        player.spriteAnim.SetBool("Hacking", true);
    }

    public override void ExitState(PlayerController player)
    {
        CloseTerminal();
        player.spriteAnim.SetBool("Hacking", false);
    }

    public override void UpdateState(PlayerController player)
    {
        player.SetPlayerVelocity(0, false);
        player.PlayerInput();
    }

    public override void FixedUpdateState(PlayerController player)
    {
        player.CheckForStealth();
    }

    public void OpenTerminal()
    {
        terminalGUI.alpha = 1;
        terminalGUI.blocksRaycasts = true;
        //if (currentTerminal.accessGranted)
        //    currentTerminal.stateMachine.ChangeState(TerminalAccessState.Instance);
        //else
        //    currentTerminal.stateMachine.ChangeState(TerminalLockedState.Instance);

    }

    public void CloseTerminal()
    {
        terminalGUI.alpha = 0;
        terminalGUI.blocksRaycasts = false;
        //currentTerminal.stateMachine.ChangeState(TerminalSleepState.Instance);
    }
}
