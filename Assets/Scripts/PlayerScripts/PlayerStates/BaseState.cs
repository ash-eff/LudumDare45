using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class BaseState : State<PlayerController>
{
    private static BaseState _instance;

    private BaseState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static BaseState Instance
    {
        get { if (_instance == null) new BaseState(); return _instance; }
    }

    public override void EnterState(PlayerController _owner)
    {
        _owner.SetPlayerSpriteActive(true);
        _owner.playerSurrounding.SetLayerToAllObstacles();
    }

    public override void ExitState(PlayerController _owner)
    {

    }

    public override void UpdateState(PlayerController _owner)
    {
        _owner.SetSpriteAnimation();
        _owner.SetSpriteDirection();
        _owner.playerSurrounding.CheckForObjects();
    }

    public override void FixedUpdateState(PlayerController _owner)
    {
        _owner.PlayerRun();
    }
}
