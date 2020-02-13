using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.StateMachine;

public class Terminal : MonoBehaviour, IInteractable
{
    public StateMachine<Terminal> stateMachine;
    public static Terminal terminal;

    public TerminalOS terminalOS;
    public bool accessGranted;

    private void Awake()
    {
        terminalOS = FindObjectOfType<TerminalOS>();
        terminal = this;
        stateMachine = new StateMachine<Terminal>(this);
        stateMachine.ChangeState(TerminalSleepState.Instance);
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();

    public void Interact()
    {

    }

    public string BeingTouched()
    {
        return "press e to hack";
    }
}
