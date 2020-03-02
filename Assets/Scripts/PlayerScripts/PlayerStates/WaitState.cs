using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class WaitState : State<PlayerController>
{
    private static WaitState _instance;

    private WaitState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance()
    {
        return Instance;
    }

    public static WaitState Instance
    {
        get { if (_instance == null) new WaitState(); return _instance; }
    }

    public override void EnterState(PlayerController player)
    {
        player.SetPlayerVelocity(0, false);
        player.SetSpriteAnimation();
        player.IdleSprite();
    }

    public override void ExitState(PlayerController player)
    {
    }

    public override void UpdateState(PlayerController player)
    {
    }

    public override void FixedUpdateState(PlayerController player)
    {
    }
}
