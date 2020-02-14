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

    public override void EnterState(PlayerController player)
    {
        player.IgnoreCollisionWithObstacles(true);
        EnterVent(player);
    }

    public override void ExitState(PlayerController player)
    {
        player.IgnoreCollisionWithObstacles(false);
        ExitVent(player);
    }

    public override void UpdateState(PlayerController player)
    {
        player.PlayerInput();
        player.SetPlayerVelocity(player.CrawlSpeed, true);
        player.SetSpriteDirection();
        player.SetSpriteAnimation();
    }

    public override void FixedUpdateState(PlayerController player)
    {
        player.CheckForObjectsOnLayer(player.ventLayer);
    }

    private void EnterVent(PlayerController player)
    {
        player.SetPlayerSpriteVisible(false);
        player.transform.position = player.currentlyTouching.GetComponentInParent<Vent>().entrance.transform.position;
        player.currentlyTouching.GetComponentInParent<Vent>().PlayerEnterVent();
        player.ventLight.gameObject.SetActive(true);
    }

    private void ExitVent(PlayerController player)
    {
        player.transform.position = player.currentlyTouching.GetComponentInParent<Vent>().exit.transform.position;
        player.currentlyTouching.GetComponentInParent<Vent>().PlayerLeaveVent();
        player.ventLight.gameObject.SetActive(false);
        player.SetPlayerSpriteVisible(true);
    }
}
