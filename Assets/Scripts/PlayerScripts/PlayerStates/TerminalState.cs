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

    TerminalOS os;
    CanvasGroup terminalGUI;

    public override void EnterState(PlayerController player)
    {
        terminalGUI = player.terminalGUI;
        os = terminalGUI.GetComponentInParent<TerminalOS>();
        //player.terminalOS.ticker.UpdateText("Entered Terminal State");
        //os.ResetOS();
        Vector3 rectLocalPos = os.GetComponent<RectTransform>().localPosition;
        os.GetComponent<RectTransform>().localPosition = new Vector3(Mathf.Abs(rectLocalPos.x) * -player.GetSpriteDirection, 0);
        player.interactText.text = "Press T to close";
        OpenTerminal();
        player.spriteAnim.SetBool("Hacking", true);
    }

    public override void ExitState(PlayerController player)
    {
        CloseTerminal();
        player.TargetRobots(false);
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
        os.stateMachine.ChangeState(TerminalBaseState.Instance);
    }

    public void CloseTerminal()
    {
        terminalGUI.alpha = 0;
        terminalGUI.blocksRaycasts = false;
        os.stateMachine.ChangeState(TerminalSleepState.Instance);
    }
}
