using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAlert : MonoBehaviour
{
    public Material robotBaseMat;
    public Material robotAlertMat;
    public Material robotAlarmMat;

    RobotSenses robotSenses;
    RobotFOV robotFOV;
    float baseVisionAngle;
    float alertAngle;
    float alarmAngle;

    private void Start()
    {
        robotSenses = GetComponent<RobotSenses>();
        robotFOV = GetComponentInChildren<RobotFOV>();
        robotFOV.GetComponent<MeshRenderer>().material = robotBaseMat;
        baseVisionAngle = robotSenses.visionAngle;
        alertAngle = robotSenses.visionAngle / 4;
        alarmAngle = robotSenses.visionAngle * 2;
    }

    //public void ReturnToStatusQuo()
    //{
    //    robotFOV.GetComponent<MeshRenderer>().material = robotBaseMat;
    //    StartCoroutine(ChangeFOVSize(robotSenses.visionAngle, baseVisionAngle));
    //}
    //
    //public void OnAlert()
    //{
    //    robotFOV.GetComponent<MeshRenderer>().material = robotAlertMat;
    //    StartCoroutine(robotSenses.CenterHead());
    //    StartCoroutine(ChangeFOVSize(robotSenses.visionAngle, alertAngle));
    //}
    //
    //public void OnAlarm()
    //{
    //    robotFOV.GetComponent<MeshRenderer>().material = robotAlarmMat;
    //    StartCoroutine(ChangeFOVSize(robotSenses.visionAngle, alarmAngle));
    //}
    //
    //IEnumerator ChangeFOVSize(float fromSize, float toSize)
    //{
    //    float lerpTime = .5f;
    //    float currentLerpTime = 0;
    //    while (fromSize != toSize)
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        float perc = currentLerpTime / lerpTime;
    //        robotSenses.visionAngle = Mathf.Lerp(fromSize, toSize, perc);
    //        yield return null;
    //    }
    //}
}
