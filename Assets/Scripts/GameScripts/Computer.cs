using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class Computer : CPU, IInteractable
{
    public float lightRebootTimer;
    public Door[] doors;
    public SingleLight[] lights;
    public bool rebootingLights;

    public void Interact()
    {

    }

    public string BeingTouched()
    {
        return "";
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

    public void AccessComputer()
    {
        player.AccessComputer(this);
    }
}
