using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class HideState : State<PlayerController>
{
    #region setup
    private static HideState _instance;

    private HideState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static HideState Instance
    {
        get { if (_instance == null) new HideState(); return _instance; }
    }
    #endregion

    Container currentContainer;

    public override void EnterState(PlayerController player)
    {
        currentContainer = player.currentlyTouching.GetComponentInParent<Container>();
        player.IgnoreCollisionWithObstacles(true);
        EnterHiding(player);
        
    }

    public override void ExitState(PlayerController player)
    {
        ExitHiding(player);
        player.IgnoreCollisionWithObstacles(false);
    }

    public override void UpdateState(PlayerController player)
    {
        player.PlayerInput();
        player.SetPlayerVelocity(0, false);
        player.SetCameraTarget(player.transform.position);
    }

    public override void FixedUpdateState(PlayerController player)
    {
        player.CheckForObjectsOnLayer(player.containerLayer);
        player.CheckForStealth();
    }

    private void EnterHiding(PlayerController player)
    {
        player.SetPlayerSpriteVisible(false);
        player.transform.position = currentContainer.entrance.transform.position;
        player.currentlyTouching.GetComponentInParent<Container>().HidingEntered();
    }

    private void ExitHiding(PlayerController player)
    {
        player.transform.position = currentContainer.exit.transform.position;
        player.SetPlayerSpriteVisible(true);
        player.currentlyTouching.GetComponentInParent<Container>().HidingExited();
    }
}
