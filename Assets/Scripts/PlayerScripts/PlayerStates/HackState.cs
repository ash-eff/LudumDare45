using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class HackState : State<PlayerController>
{
    #region setup
    private static HackState _instance;

    private HackState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static HackState Instance
    {
        get { if (_instance == null) new HackState(); return _instance; }
    }
    #endregion

    Terminal currentTerminal;
    CanvasGroup terminalGUI;

    public override void EnterState(PlayerController player)
    {
        terminalGUI = player.terminalGUI;
        currentTerminal = player.currentlyTouching.GetComponent<Terminal>();
        player.interactText.text = "Press E to close";
        OpenTerminal();
    }

    public override void ExitState(PlayerController player)
    {
        player.TargetRobots(false);
        CloseTerminal();
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
        if (currentTerminal.accessGranted)
            currentTerminal.stateMachine.ChangeState(TerminalAccessState.Instance);
        else
            currentTerminal.stateMachine.ChangeState(TerminalLockedState.Instance);

    }

    public void CloseTerminal()
    {
        terminalGUI.alpha = 0;
        terminalGUI.blocksRaycasts = false;
        currentTerminal.stateMachine.ChangeState(TerminalSleepState.Instance);
    }
}
