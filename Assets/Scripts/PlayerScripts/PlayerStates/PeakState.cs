using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class PeakState : State<PlayerController>
{
    #region setup
    private static PeakState _instance;

    private PeakState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static PeakState Instance
    {
        get { if (_instance == null) new PeakState(); return _instance; }
    }
    #endregion

    RoomExit roomExit;

    public override void EnterState(PlayerController player)
    {
        player.interactText.text = "";
        roomExit = player.currentlyTouching.GetComponent<RoomExit>();
        roomExit.PeakIntoRoom();
        player.SetPlayerSpriteVisible(false);
    }

    public override void ExitState(PlayerController player)
    {
        roomExit.ResetPeak();
        player.SetPlayerSpriteVisible(true);
    }

    public override void UpdateState(PlayerController player)
    {
        player.SetCameraTarget(roomExit.nextRoomEntrance.transform.position);
        player.PlayerInput();
        if(player.Movement.x != 0 || player.Movement.y != 0)
        {
            player.stateMachine.ChangeState(BaseState.Instance);
        }
    }

    public override void FixedUpdateState(PlayerController player)
    {
    }
}
