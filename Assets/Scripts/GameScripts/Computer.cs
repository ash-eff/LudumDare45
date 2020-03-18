using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.PlayerController;

public class Computer : MonoBehaviour, IInteractable
{
    //public StateMachine<Computer> stateMachine;
    //public static Computer computer;

    public TerminalOS terminalOS;
    public bool accessGranted;
    public float lightRebootTimer;
    public Door[] doors;
    public SingleLight[] lights;
    //public GameObject lightWarning;
    //public Image lightWarningFill;

    public PlayerController player;
    public bool rebootingLights;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        terminalOS = FindObjectOfType<TerminalOS>();
        //computer = this;
        //stateMachine = new StateMachine<Computer>(this);
        //stateMachine.ChangeState(TerminalSleepState.Instance);
    }

    //private void Update() => stateMachine.Update();
    //private void FixedUpdate() => stateMachine.FixedUpdate();

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

    public float DistanceFromPlayer()
    {
        float distance = (player.transform.position - transform.position).magnitude;

        return distance;
    }
}
