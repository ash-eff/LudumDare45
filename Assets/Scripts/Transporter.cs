using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    public string transporterInfo;
    public GameObject exitLocation;
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }
}
