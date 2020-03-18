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

    Computer workingComputer;
    CanvasGroup terminalGUI;
    PlayerController player;

    public override void EnterState(PlayerController _player)
    {
        player = _player;
        terminalGUI = player.terminalGUI;
        workingComputer = player.currentlyTouching.GetComponent<Computer>();
        player.interactText.text = "Press T to close";
        OpenTerminal();
    }

    public override void ExitState(PlayerController _player)
    {
        CloseTerminal();
    }

    public override void UpdateState(PlayerController _player)
    {
        player.SetPlayerVelocity(0, false);
        player.PlayerInput();
    }

    public override void FixedUpdateState(PlayerController _player)
    {
        player.CheckForStealth();
    }

    public void OpenTerminal()
    {
        terminalGUI.alpha = 1;
        terminalGUI.blocksRaycasts = true;         
        player.terminalOS.stateMachine.ChangeState(TerminalAccessState.Instance);
    }

    public void CloseTerminal()
    {
        terminalGUI.alpha = 0;
        terminalGUI.blocksRaycasts = false;
        player.terminalOS.stateMachine.ChangeState(TerminalSleepState.Instance);
    }
}
