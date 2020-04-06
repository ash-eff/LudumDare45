using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hackable : MonoBehaviour
{
    public int passwordStrength;
    public int dataEncryption;
    public int softwareMaintenance;

    public bool accessGranted;

    public bool AccessGranted { get { return accessGranted; } }

    public void Hack()
    {

    }
}
