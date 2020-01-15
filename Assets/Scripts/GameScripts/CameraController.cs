using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    public bool followPlayer;
    private GameController gameController;
    private PlayerManager playerManager;
    public GameObject playerCursor;
    Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerManager = FindObjectOfType<PlayerManager>();
        gameController = FindObjectOfType<GameController>();
        if (followPlayer)
        {
           rb2d.position = new Vector3(playerManager.transform.position.x, playerManager.transform.position.y, -10f);
        }
    }

    private void FixedUpdate()
    {
        if (followPlayer)
        {
            if (gameController.IsGameOver)
            {
                return;
            }

            if (playerManager.CanMove)
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
        Vector3 targetPos = new Vector3(playerManager.transform.position.x, playerManager.transform.position.y, -10f);
        rb2d.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    }

    void FloatCameraTowardCursor()
    {
        Vector3 targetPos = new Vector3(playerCursor.transform.position.x, playerCursor.transform.position.y, -10f);
        rb2d.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    }
}
