using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.PlayerController;

public class Computer : MonoBehaviour, IInteractable
{
    public TerminalOS terminalOS;
    public bool accessGranted;
    public float lightRebootTimer;
    public Door[] doors;
    public SingleLight[] lights;
    public GameObject link;
    public GameObject ring;
    public Button button;

    public PlayerController player;
    public bool rebootingLights;

    private AudioSource audioSource;
    private bool pinged;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();
        terminalOS = FindObjectOfType<TerminalOS>();
    }

    private void Update()
    {
        if(DistanceFromPlayer() > terminalOS.terminalRange)
        {
            pinged = false;
            link.gameObject.SetActive(false);
            ring.SetActive(false);
            button.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {

    }

    public string BeingTouched()
    {
        return "press t to hack";
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

    public void PingComputer()
    {
        if (!pinged)
        {
            pinged = true;
            audioSource.Play();
            StartCoroutine(MoveLink());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, terminalOS.terminalRange);
    }

    IEnumerator MoveLink()
    {
        link.gameObject.SetActive(false);
        ring.SetActive(false);
        button.gameObject.SetActive(false);
        link.transform.position = player.transform.position;
        link.gameObject.SetActive(true);
        Vector3 directionTo = transform.position;
        while (link.transform.position != directionTo)
        {
            link.transform.position = Vector3.MoveTowards(link.transform.position, directionTo, 55f * Time.deltaTime);
            yield return null;
        }

        link.gameObject.SetActive(false);
        ring.SetActive(true);
        button.gameObject.SetActive(true);
    }

    public void AccessComputer()
    {
        player.AccessComputer(this);
    }
}
