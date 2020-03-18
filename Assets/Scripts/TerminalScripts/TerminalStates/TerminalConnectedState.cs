﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalConnectedState : State<TerminalOS>
{
    #region setup
    private static TerminalConnectedState _instance;

    private TerminalConnectedState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<TerminalOS> createInstance() { return Instance; }

    public static TerminalConnectedState Instance
    {
        get { if (_instance == null) new TerminalConnectedState(); return _instance; }
    }
    #endregion

    public override void EnterState(TerminalOS terminalOS)
    {
        terminalOS.terminalAccessIcon.GetComponent<Button>().interactable = true;
        terminalOS.terminalAccessIcon.SetActive(true);
        // set other icons active here
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
