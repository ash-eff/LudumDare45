using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float interactTime;
    public float moveSpeed;

    private bool playerOccupied;

    private Rigidbody2D rb2d;
    private PlayerManager playerManager;
    private Vector2 movement;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (playerManager.IsSpotted || playerOccupied)
        {
            return;
        }

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        CheckForInteractions();
    }

    private void FixedUpdate()
    {
        if (playerManager.IsSpotted || playerOccupied)
        {
            return;
        }

        MovePlayer(movement);
    }

    void MovePlayer(Vector2 direction)
    {
        rb2d.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime)); 
    }

    void CheckForInteractions()
    {
        if (playerManager.CanInteract && !playerOccupied)
        {
            if (Input.GetButtonDown("Interact"))
            {
                playerOccupied = true;
                StartCoroutine(InteractWithItem());
            }
        }
    }

     IEnumerator InteractWithItem()
    {
        float timeOfInteraction = interactTime;
        playerManager.interactTimeFill.fillAmount = 0f;
        while (Input.GetButton("Interact"))
        {
            timeOfInteraction -= Time.deltaTime;
            playerManager.interactTimeFill.fillAmount += Time.deltaTime / interactTime;

            if (timeOfInteraction <= 0)
            {
                playerOccupied = false;
                playerManager.StealMoney();
                break;
            }

            yield return null;
        }

        playerManager.interactTimeFill.fillAmount = 0f;
        playerOccupied = false;
    }
}
