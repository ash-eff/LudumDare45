using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    private GameController gameController;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameController = FindObjectOfType<GameController>();
        transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, -10f);  
    }

    private void FixedUpdate()
    {
        if (gameController.IsGameOver)
        {
            return;
        }

        if (!playerController.IsTeleporting)
        {
            FollowPlayer();
        }
        else
        {
            transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, -10f);
        }
    }

    void FollowPlayer()
    {
        Vector3 targetPos = new Vector3(playerController.transform.position.x, playerController.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
