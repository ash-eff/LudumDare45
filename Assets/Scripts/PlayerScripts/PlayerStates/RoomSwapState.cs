﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class RoomSwapState : State<PlayerController>
{
    #region setup
    private static RoomSwapState _instance;

    private RoomSwapState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static RoomSwapState Instance
    {
        get { if (_instance == null) new RoomSwapState(); return _instance; }
    }
    #endregion

    public override void EnterState(PlayerController player)
    {

        player.IdleSprite();
        player.lightsInArea = player.gameController.currentRoom.GetComponentsInChildren<SingleLight>();
        player.transform.position = player.gameController.roomEntrance.transform.position;
    }

    public override void ExitState(PlayerController player)
    {
        
    }

    public override void UpdateState(PlayerController player)
    {
        player.SetCameraTarget(player.transform.position);

    }

    public override void FixedUpdateState(PlayerController player)
    {
    }
}
