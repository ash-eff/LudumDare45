using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class PlayerCursor : MonoBehaviour
{
    public Texture2D targetTexture;
    public Texture2D pointerTexture;
    private PlayerController player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        Cursor.SetCursor(targetTexture, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void Update()
    {
        CursorIcon();
    }

    void CursorIcon()
    {
        if (player.stateMachine.currentState != HackState.Instance && player.stateMachine.currentState != TerminalState.Instance)
            Cursor.SetCursor(targetTexture, Vector2.zero, CursorMode.ForceSoftware);
        else
            Cursor.SetCursor(pointerTexture, new Vector2(-1, 1), CursorMode.ForceSoftware);
    }
}
