using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    public bool followPlayer;
    private GameController gameController;
    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        gameController = FindObjectOfType<GameController>();
        if (followPlayer)
        {
            transform.position = new Vector3(playerManager.transform.position.x, playerManager.transform.position.y, -10f);
        }
    }

    private void LateUpdate()
    {
        if (followPlayer)
        {
            if (gameController.IsGameOver)
            {
                return;
            }

            if (!playerManager.IsTeleporting)
            {
                FollowPlayer();
            }
            else
            {
                transform.position = new Vector3(playerManager.transform.position.x, playerManager.transform.position.y, -10f);
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 targetPos = new Vector3(playerManager.transform.position.x, playerManager.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
