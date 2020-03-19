using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class BaseState : State<PlayerController>
{
    #region setup
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
    #endregion

    public override void EnterState(PlayerController player)
    {
        //player.terminalOS.ticker.UpdateText("Entered Base State"); 
        player.SetPlayerSpriteVisible(true);
        player.layersToCheck = player.allObstacleLayers;
    }

    public override void ExitState(PlayerController player)
    {
    }

    public override void UpdateState(PlayerController player)
    {
        //player.GetComponentInChildren<SpriteRenderer>().
        player.PlayerInput();
        player.SetPlayerVelocity(player.RunSpeed, true);
        player.SetSpriteDirection();
        player.SetSpriteAnimation();
        player.WarningAlertIndicator();
        player.SetCameraTarget(player.transform.position);
    }

    public override void FixedUpdateState(PlayerController player)
    {
        player.CheckForObjectsOnLayer(player.allObstacleLayers);
        player.CheckForStealth();
    }
}
