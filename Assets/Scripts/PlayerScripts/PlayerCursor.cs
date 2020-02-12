using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        CursorPos();
    }

    public void CursorPos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.parent.position;

        direction = Vector2.ClampMagnitude(direction, 9);

        transform.position = (Vector2)transform.parent.position + direction;
    }
}
