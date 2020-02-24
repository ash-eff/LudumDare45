using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPatrol : MonoBehaviour
{
    RobotController robot;

    private void Awake()
    {
        robot = GetComponent<RobotController>();
    }


    //IEnumerator SpeedUp()
    //{
    //    float startSpeed = .25f;
    //    float lerpTime = 1f;
    //    float currentLerpTime = 0;
    //
    //    while (currentSpeed < moveSpeed)
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        if (currentLerpTime > lerpTime)
    //        {
    //            currentLerpTime = lerpTime;
    //        }
    //
    //        float perc = currentLerpTime / lerpTime;
    //        currentSpeed = Mathf.Lerp(startSpeed, moveSpeed, perc);
    //
    //        yield return null;
    //    }
    //}
    //
    //IEnumerator SlowDown(float byTime)
    //{
    //    float startSpeed = currentSpeed;
    //    float endSpeed = .25f;
    //    float lerpTime = byTime;
    //    float currentLerpTime = 0;
    //
    //    while (currentSpeed > endSpeed)
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        if (currentLerpTime > lerpTime)
    //        {
    //            currentLerpTime = lerpTime;
    //        }
    //
    //        float perc = currentLerpTime / lerpTime;
    //        currentSpeed = Mathf.Lerp(startSpeed, endSpeed, perc);
    //
    //        yield return null;
    //    }
    //}

    //public IEnumerator RotateTowardsTarget(Vector2 target)
    //{
    //    float lerpTime = .2f;
    //    float currentLerpTime = 0;
    //    float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
    //    float startingRot = transform.localEulerAngles.z;
    //
    //    if (startingRot > 180)
    //    {
    //        startingRot -= 360f;
    //    }
    //
    //    while (startingRot != Mathf.Abs(angle))
    //    {
    //        currentLerpTime += Time.deltaTime;
    //        float perc = currentLerpTime / lerpTime;
    //        float diff = Mathf.LerpAngle(startingRot, angle, perc);
    //        transform.rotation = Quaternion.Euler(0f, 0f, diff);
    //
    //        yield return null;
    //    }
    //}
}
