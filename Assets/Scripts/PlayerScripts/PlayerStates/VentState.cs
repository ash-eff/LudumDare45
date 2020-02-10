using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class VentState : State<PlayerController>
{
    private static VentState _instance;

    private VentState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance()
    {
        return Instance;
    }

    public static VentState Instance
    {
        get { if (_instance == null) new VentState(); return _instance; }
    }

    public override void EnterState(PlayerController _owner)
    {
        _owner.SetPlayerSpriteActive(false);
        _owner.playerSurrounding.SetLayerToVentsLayer();
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
        _owner.PlayerCrawl();
    }
}
