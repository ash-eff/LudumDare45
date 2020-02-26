using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapScript : MonoBehaviour
{
    private void Update()
    {
        transform.position = MyUtils.SnapToGrid(transform.position);
    }
}
