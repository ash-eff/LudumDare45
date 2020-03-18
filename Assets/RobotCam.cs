using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCam : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target != null)
        {
            FollowPlayerTarget(target);
        }
    }

    void FollowPlayerTarget(Transform _target)
    {
        Vector3 targetPos = new Vector3(_target.position.x, _target.position.y, -10f);
        transform.position = targetPos;
    }
}
