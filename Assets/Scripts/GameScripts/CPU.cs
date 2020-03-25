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
    public GameObject iconPositionObject;
    public GameObject cpuLink;
    public GameObject cpuRing;
    public GameObject cpuWindow;
    public Canvas canvas;
    public Button cpuButton;
    public PlayerController player;
    public AudioSource audioSource;
    public bool pinged;

    public GameObject[] iconPositions;

    public GameObject lightsIcon;
    public GameObject floorplanIcon;
    public GameObject locksIcon;
    public GameObject firewallIcon;
    public GameObject reverseIcon;
    public GameObject bombIcon;
    public GameObject friendsIcon;
    public GameObject powerIcon;

    public List<GameObject> activeIcons = new List<GameObject>();
    LineRenderer lr;

    protected virtual void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();
        terminalOS = FindObjectOfType<TerminalOS>();
        processingUnit = transform.Find("CPU").gameObject;
        cpuLink = processingUnit.transform.Find("CPU Link").gameObject;
        cpuRing = processingUnit.transform.Find("CPU Ring").gameObject;
        canvas = GetComponentInChildren<Canvas>();
        //canvas.worldCamera = Camera.main;
        cpuWindow = canvas.transform.Find("CPU Window").gameObject;
        cpuButton = processingUnit.GetComponentInChildren<Button>();
        audioSource = processingUnit.GetComponent<AudioSource>();
        lr = cpuLink.transform.GetComponent<LineRenderer>();
        cpuLink.SetActive(false);
        cpuRing.SetActive(false);
        cpuButton.gameObject.SetActive(false);
        //IconSetup();
    }

    protected virtual void Update()
    {
        CheckForPing();
    }

    public void CheckForPing()
    {
        if (DistanceFromPlayer() <= terminalOS.terminalRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(cpuRing.transform.position, (player.transform.position - cpuRing.transform.position).normalized, terminalOS.terminalRange, visionLayers);
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

    public void IconSetup()
    {
        int iconIndex = 0;
        foreach (GameObject icon in activeIcons)
        {
            icon.transform.localPosition = iconPositions[iconIndex].transform.localPosition;
            icon.SetActive(true);
            iconIndex++;
        }
    }

    public float DistanceFromPlayer()
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
        Color color = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, 1);
        lr.startColor = color;
        lr.endColor = color;
        cpuLink.transform.parent = null;
        cpuLink.gameObject.SetActive(false);
        cpuRing.SetActive(false);
        cpuButton.gameObject.SetActive(false);
        cpuLink.transform.position = player.transform.position;
        Vector3 finalPosition = CalculateLineFinalPosition();
        Vector3 linkEndPos = player.transform.position;
        cpuLink.gameObject.SetActive(true);
        lr.SetPosition(0, player.transform.position);
        lr.SetPosition(1, player.transform.position);

        while (lr.GetPosition(1) != finalPosition)
        {
            lr.SetPosition(0, player.transform.position);
            lr.SetPosition(1, linkEndPos);
            linkEndPos = Vector3.MoveTowards(linkEndPos, finalPosition, 55f * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FadeLink());
        cpuRing.SetActive(true);
        cpuButton.gameObject.SetActive(true);
    }

    IEnumerator FadeLink()
    {
        Color a = lr.startColor;
        Color b = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, 0);
        float timer = 1f;
        float lerptime = timer;
        float currentLerpTime = 0;
        while (timer > 0 && pinged)
        {
            timer -= Time.deltaTime;
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerptime;
            lr.startColor = Color.Lerp(a, b, perc);
            lr.endColor = Color.Lerp(a, b, perc);
            lr.SetPosition(0, player.transform.position);
            lr.SetPosition(1, CalculateLineFinalPosition());

            yield return null;
        }

        cpuLink.gameObject.SetActive(false);
        cpuLink.transform.parent = processingUnit.transform;
    }

    private Vector3 CalculateLineFinalPosition()
    {
        Vector3 direction = (cpuRing.transform.position - player.transform.position).normalized;
        float distance = (cpuRing.transform.position - player.transform.position).magnitude;
        Vector3 finalPosition = player.transform.position + (direction * (distance - cpuRing.transform.localScale.x));

        return finalPosition;
    }
}
