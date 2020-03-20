using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.PlayerController;

public class CPU : MonoBehaviour
{
    public LayerMask visionLayers;
    public TerminalOS terminalOS;
    public bool accessGranted;
    public GameObject processingUnit;
    public GameObject cpuLink;
    public GameObject cpuRing;
    public Button cpuButton;
    public PlayerController player;
    public AudioSource audioSource;
    public bool pinged;

    protected virtual void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();
        terminalOS = FindObjectOfType<TerminalOS>();
        processingUnit = transform.Find("CPU").gameObject;
        cpuLink = processingUnit.transform.Find("CPU Link").gameObject;
        cpuRing = processingUnit.transform.Find("CPU Ring").gameObject;
        cpuButton = processingUnit.GetComponentInChildren<Button>();
        audioSource = processingUnit.GetComponent<AudioSource>();
        cpuLink.SetActive(false);
        cpuRing.SetActive(false);
        cpuButton.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        CheckForPing();
    }

    public void CheckForPing()
    {
        if (DistanceFromPlayer() <= terminalOS.terminalRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(processingUnit.transform.position, (player.transform.position - processingUnit.transform.position).normalized, terminalOS.terminalRange, visionLayers);
            if (hit)
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                    PingCPU();
                }
                else
                {
                    pinged = false;
                    cpuLink.gameObject.SetActive(false);
                    cpuRing.SetActive(false);
                    cpuButton.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            pinged = false;
            cpuLink.gameObject.SetActive(false);
            cpuRing.SetActive(false);
            cpuButton.gameObject.SetActive(false);
        }
    }

    private float DistanceFromPlayer()
    {
        float distance = (player.transform.position - transform.position).magnitude;
        return distance;
    }

    private void PingCPU()
    {
        if (!pinged)
        {
            pinged = true;
            audioSource.Play();
            StartCoroutine(MoveLink());
        }
    }

    IEnumerator MoveLink()
    {
        cpuLink.transform.parent = null;
        cpuLink.gameObject.SetActive(false);
        cpuRing.SetActive(false);
        cpuButton.gameObject.SetActive(false);
        cpuLink.transform.position = player.transform.position;
        Vector3 directionTo = processingUnit.transform.position;
        cpuLink.gameObject.SetActive(true);

        while (cpuLink.transform.position != directionTo)
        {
            cpuLink.transform.position = Vector3.MoveTowards(cpuLink.transform.position, directionTo, 55f * Time.deltaTime);
            yield return null;
        }

        
        cpuLink.gameObject.SetActive(false);
        cpuLink.transform.parent = processingUnit.transform;
        cpuRing.SetActive(true);
        cpuButton.gameObject.SetActive(true);
    }
}
