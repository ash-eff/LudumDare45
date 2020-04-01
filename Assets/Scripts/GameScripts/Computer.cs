using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class Computer : MonoBehaviour, IHackable
{
    public float lightRebootTimer;
    public Door[] doors;
    public SingleLight[] lights;
    public bool rebootingLights;
    public bool isHacked;

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
        //cpu.terminalOS.SetWorkingCPU(this);
    }

    public bool AccessGranted()
    {
        return isHacked;
    }

    public void Hack()
    {
        isHacked = true;
    }

    public GameObject GetAttachedGameObject()
    {
        return gameObject; 
    }

    public string GetSystemName()
    {
        return "FL SYS 776";
    }
}
