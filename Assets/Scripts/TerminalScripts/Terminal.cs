using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.StateMachine;

public class Terminal : MonoBehaviour, IInteractable
{
    public StateMachine<Terminal> stateMachine;
    public static Terminal terminal;

    public TerminalOS terminalOS;
    public bool accessGranted;
    public float lightRebootTimer;
    public Door[] doors;
    public SingleLight[] lights;
    //public GameObject lightWarning;
    //public Image lightWarningFill;

    public bool rebootingLights;

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

    public void UseLights()
    {
        if (!rebootingLights)
        {
            rebootingLights = true;
            foreach (SingleLight light in lights)
            {
                light.DeactivateLights();
            }

            StartCoroutine(RebootLights());
        }
    }

    private IEnumerator RebootLights()
    {
        //lightWarning.SetActive(true);
        //lightWarningFill.fillAmount = 1;
        float timer = lightRebootTimer;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            //lightWarningFill.fillAmount -= Time.deltaTime / lightRebootTimer;

            yield return null;
        }

        //lightWarning.SetActive(false);
        rebootingLights = false;
        foreach (SingleLight light in lights)
        {
            light.ActivateLights();
        }
    }
}
