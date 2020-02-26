using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[ExecuteInEditMode]
public class SingleLight : MonoBehaviour
{
    public Light2D pointLight;
    public Light2D freeformLight;
    public bool isActive = true;
    public float lightRadius;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, lightRadius);
    }
}
