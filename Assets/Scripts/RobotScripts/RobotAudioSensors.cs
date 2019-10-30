using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAudioSensors : MonoBehaviour
{
    private Vector2Int noiseLocation;

    public void RobotHeardANoise(Vector2 atPosition)
    {
        noiseLocation = new Vector2Int((int)atPosition.x, (int)atPosition.y);
        Debug.Log(transform.name + " heard a noise at position: " + noiseLocation);
        Debug.DrawRay(transform.position, (Vector3)atPosition - transform.position, Color.green, 1f);
        //StartCoroutine(robotMove.Stop(RobotPathing.State.InvestigateState));      
    }
}
