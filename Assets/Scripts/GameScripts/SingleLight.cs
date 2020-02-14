using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SingleLight : MonoBehaviour
{
    public Light2D pointLight;
    public Light2D freeformLight;
    public bool isActive = true;

    public void ActivateLights()
    {
        isActive = true;
        pointLight.enabled = true;
        freeformLight.enabled = true;
    }

    public void DeactivateLights()
    {
        isActive = false;
        pointLight.enabled = false;
        freeformLight.enabled = false;
    }
}
