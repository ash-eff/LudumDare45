using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    public bool followPlayer;
    private GameController gameController;
    private PlayerController player;
    //private Vector2 playerCursorPos;

    private void Awake()
    {
        player = FindObjectOfType<Ash.PlayerController.PlayerController>();
        //playerCursorPos = player.GetCursorPos;
        gameController = FindObjectOfType<GameController>();
        if (followPlayer)
        {
           transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        }
    }

    private void FixedUpdate()
    {
        if (followPlayer)
        {
            //if (gameController.IsGameOver)
            //{
            //    return;
            //}
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    }

    //void FloatCameraTowardCursor()
    //{
    //    Vector3 targetPos = new Vector3(playerCursorPos.x, playerCursorPos.y, -10f);
    //    transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    //}
}
