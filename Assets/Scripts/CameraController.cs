using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    private GameController gameController;
    private PlayerController playerTarget;

    private void Start()
    {
        playerTarget = FindObjectOfType<PlayerController>();
        transform.position = playerTarget.transform.position;
        gameController = FindObjectOfType<GameController>();
    }

    private void FixedUpdate()
    {
        if (gameController.IsGameOver)
        {
            return;
        }

        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPos = new Vector3(playerTarget.transform.position.x, playerTarget.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
