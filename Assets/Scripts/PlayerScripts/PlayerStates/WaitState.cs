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

    public override void EnterState(PlayerController _owner)
    {
        _owner.SetPlayerSpriteActive(true);
        _owner.SetSpriteAnimation();
        _owner.playerSurrounding.SetLayerToAllObstacles();
    }

    public override void ExitState(PlayerController _owner)
    {

    }

    public override void UpdateState(PlayerController _owner)
    {
        _owner.playerSurrounding.CheckForObjects();
    }

    public override void FixedUpdateState(PlayerController _owner)
    {
        _owner.StopPlayer();
    }
}
