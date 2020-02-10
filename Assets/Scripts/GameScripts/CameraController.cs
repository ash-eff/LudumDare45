using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    public bool followPlayer;
    private GameController gameController;
    private Ash.PlayerController.PlayerController playerController;
    public GameObject playerCursor;

    private void Awake()
    {
        playerController = FindObjectOfType<Ash.PlayerController.PlayerController>();
        gameController = FindObjectOfType<GameController>();
        if (followPlayer)
        {
           transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, -10f);
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

            if (playerController.CanMove)
            {
                FollowPlayer();
            }
            else
            {
                FloatCameraTowardCursor();
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 targetPos = new Vector3(playerController.transform.position.x, playerController.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    }

    void FloatCameraTowardCursor()
    {
        Vector3 targetPos = new Vector3(playerCursor.transform.position.x, playerCursor.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    }
}
