using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;

public class TerminalSleepState : State<TerminalOS>
{
    #region setup
    private static TerminalSleepState _instance;

    private TerminalSleepState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<TerminalOS> createInstance() { return Instance; }

    public static TerminalSleepState Instance
    {
        get { if (_instance == null) new TerminalSleepState(); return _instance; }
    }
    #endregion

    public override void EnterState(TerminalOS terminalOS)
    {
        terminalOS.ResetOS();
    }

    public override void ExitState(TerminalOS terminalOS)
    {
        
    }

    public override void UpdateState(TerminalOS terminalOS)
    {
        terminalOS.SignalStrength();
        terminalOS.IsComputerAccessible();
        terminalOS.CheckForComputerInRange();
        //if (terminalOS.CheckForComputerInRange())
        //{
        //    terminalOS.ticker.UpdateText("Computer in range");
        //}
        //else
        //{
        //    terminalOS.ticker.UpdateText("HackerBox VErsion 1.8 is available now");
        //}
    }

    public override void FixedUpdateState(TerminalOS terminalOS)
    {
    }
}
