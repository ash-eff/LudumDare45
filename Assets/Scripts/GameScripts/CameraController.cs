using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    public bool followPlayer;
    public GameObject mainCam;
    private GameController gameController;
    private PlayerController player;
    public Vector3 targetPos;
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
            FollowPlayerTarget(player.cameraTarget);
        }
    }

    void FollowPlayerTarget(Vector2 _target)
    {
        targetPos = new Vector3(_target.x, _target.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    }

    //void FloatCameraTowardCursor()
    //{
    //    Vector3 targetPos = new Vector3(playerCursorPos.x, playerCursorPos.y, -10f);
    //    transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    //}

    public void CameraShake()
    {
        StartCoroutine(Shake(.25f, .1f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = mainCam.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            mainCam.transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCam.transform.localPosition = originalPos;
    }
}
