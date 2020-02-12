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
    }

    public override void ExitState(PlayerController _owner)
    {
    }

    public override void UpdateState(PlayerController _owner)
    {
    }

    public override void FixedUpdateState(PlayerController _owner)
    {
    }
}
