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
    CanvasGroup terminalGUI;
    PlayerController player;

    public override void EnterState(PlayerController _player)
    {
        player = _player;
        terminalGUI = player.terminalGUI;
        Vector3 rectLocalPos = player.terminalOS.GetComponent<RectTransform>().localPosition;
        player.terminalOS.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Abs(rectLocalPos.x) * -player.GetSpriteDirection, 0);
        player.interactText.text = "Press T to close";
        OpenTerminal();
        player.spriteAnim.SetBool("Hacking", true);
    }

    public override void ExitState(PlayerController _player)
    {
        CloseTerminal();
        player.spriteAnim.SetBool("Hacking", false);
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
