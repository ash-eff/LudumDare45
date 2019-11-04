using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAlert : MonoBehaviour
{
    public Color baseFovColor;
    public Color alertFovColor;
    public Color alarmFovColor;

    RobotSenses robotSenses;
    RobotFOV robotFOV;
    float baseVisionAngle;
    float alertAngle;
    float alarmAngle;

    private void Start()
    {
        robotSenses = GetComponent<RobotSenses>();
        robotFOV = GetComponentInChildren<RobotFOV>();
        robotFOV.mat.color = baseFovColor;
        baseVisionAngle = robotSenses.visionAngle;
        alertAngle = robotSenses.visionAngle / 4;
        alarmAngle = robotSenses.visionAngle * 2;
    }

    public void ReturnToStatusQuo()
    {
        robotFOV.mat.color = baseFovColor;
        StartCoroutine(ChangeFOVSize(robotSenses.visionAngle, baseVisionAngle));
    }

    public void OnAlert()
    {
        robotFOV.mat.color = alertFovColor;
        StartCoroutine(robotSenses.CenterHead());
        StartCoroutine(ChangeFOVSize(robotSenses.visionAngle, alertAngle));
    }

    public void OnAlarm()
    {
        robotFOV.mat.color = alarmFovColor;
        StartCoroutine(ChangeFOVSize(robotSenses.visionAngle, alarmAngle));
    }

    IEnumerator ChangeFOVSize(float fromSize, float toSize)
    {
        float lerpTime = .5f;
        float currentLerpTime = 0;
        while (fromSize != toSize)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            robotSenses.visionAngle = Mathf.Lerp(fromSize, toSize, perc);
            yield return null;
        }
    }
}
