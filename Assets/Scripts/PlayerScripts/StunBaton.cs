using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBaton : MonoBehaviour
{
    public Animator anim;
    //public GameObject baton;

    private void Update()
    {
        BatonPos();
    }

    public void BatonPos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.parent.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //transform.position = (Vector2)transform.parent.position + direction;
    }

    public void Swing()
    {
        Debug.Log("Swing");
        anim.SetTrigger("Swing");
    }

    public void StopSwing()
    {
        Debug.Log("Stop");
        anim.ResetTrigger("Swing");
    }
}
