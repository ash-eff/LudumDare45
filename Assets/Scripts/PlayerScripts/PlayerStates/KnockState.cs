using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class KnockState : State<PlayerController>
{
    #region setup
    private static KnockState _instance;

    private KnockState()
    {
        if (_instance != null) return;
        _instance = this;
    }

    public override State<PlayerController> createInstance() { return Instance; }

    public static KnockState Instance
    {
        get { if (_instance == null) new KnockState(); return _instance; }
    }
    #endregion

    float timer;
    Noise noisePrefab;

    public override void EnterState(PlayerController player)
    {
        player.interactText.text = "";
        noisePrefab = player.noisePrefab;
        timer = 1f;
        InstantiateKnock(player.transform.position);
    }

    public override void ExitState(PlayerController player)
    {       
    }

    public override void UpdateState(PlayerController player)
    {
        player.PlayerInput();
        player.SetPlayerVelocity(player.RunSpeed, true);
        player.SetSpriteDirection();
        player.SetSpriteAnimation();
        timer -= Time.deltaTime;
        if (timer <= 0)
            player.stateMachine.ChangeState(BaseState.Instance);
    }

    public override void FixedUpdateState(PlayerController player)
    {       
    }

    private void InstantiateKnock(Vector2 _atPosition)
    {
        GameObject.Instantiate(noisePrefab, _atPosition, Quaternion.identity);
    }
}
