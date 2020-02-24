using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class PlayerCursor : MonoBehaviour
{
    public Sprite target;
    public Sprite arrow;

    private SpriteRenderer spr;
    private PlayerController player;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<PlayerController>();
        //Cursor.visible = false;
    }

    private void Update()
    {
        CursorPos();
        CursorIcon();
    }

    public void CursorPos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - (Vector2)transform.parent.position;

        if(player.stateMachine.currentState != HackState.Instance)
            direction = Vector2.ClampMagnitude(direction, 9);

        transform.position = (Vector2)transform.parent.position + direction;
    }

    void CursorIcon()
    {
        if (player.stateMachine.currentState != HackState.Instance)
            spr.sprite = target;
        else
            spr.sprite = arrow;
    }
}
