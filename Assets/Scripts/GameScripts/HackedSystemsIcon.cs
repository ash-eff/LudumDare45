using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.PlayerController;

public class HackedSystemsIcon : MonoBehaviour
{
    public TerminalOS terminalOS;
    public GameObject nullIcon;
    public Computer computer;
    public PlayerController player;
    public Button iconButton;

    private float distance;

    private void Awake()
    {
        iconButton = GetComponent<Button>();
        player = FindObjectOfType<PlayerController>();
        terminalOS = GetComponentInParent<TerminalOS>();
    }

    private void Update()
    {
        if(computer != null)
        {
            distance = (computer.transform.position - player.transform.position).magnitude;
            if (distance < 10f)
            {
                iconButton.interactable = true;
                nullIcon.SetActive(false);
            }
            else
            {
                iconButton.interactable = false;
                nullIcon.SetActive(true);
            }
        }
        else
        {
            iconButton.interactable = false;
        }
    }

    public void AccessComputerWindow()
    {
        if(distance < 10f)
        {
            terminalOS.workingComputer = computer;
            terminalOS.terminalAccessIcon.SetActive(true);
            terminalOS.OpenTerminalAccessWindow();
        }
        else
        {
            Debug.Log("Not in range");
        }
    }
}
