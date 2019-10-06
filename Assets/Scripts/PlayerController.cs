using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float stealTime;
    public float moveSpeed;

    private bool playerOccupied;
    private bool isTeleporting;

    private Rigidbody2D rb2d;
    private PlayerManager playerManager;
    private Vector2 movement;

    public bool IsTeleporting { get { return isTeleporting; } }

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
        CheckForButtonPress();
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

    IEnumerator MovePlayerToNewFloor(Vector2 location)
    {
        StartCoroutine(playerManager.SetPlayerUntargetable(4f));
        StartCoroutine(playerManager.HidePlayerTeleport());
        yield return new WaitForSecondsRealtime(1f);
        isTeleporting = true;
        transform.position = location;
        playerManager.CheckFloor();
        yield return new WaitForSecondsRealtime(1.5f);
        isTeleporting = false;
        playerOccupied = false;
    }

    void CheckForButtonPress()
    {
        if (playerManager.CanInteract && !playerOccupied)
        {
            if (Input.GetButtonDown("Interact"))
            {
                playerOccupied = true;
                StartCoroutine(InteractWithItem());
            }
        }

        if (playerManager.CanTeleport && !playerOccupied)
        {
            if (Input.GetButtonDown("Interact"))
            {
                playerManager.CancelTextInformation();
                playerOccupied = true;
                StartCoroutine(MovePlayerToNewFloor(playerManager.CurrentTransporter.exitLocation.transform.position));
            }
        }
    }

    IEnumerator InteractWithItem()
    {
        float timeOfInteraction = stealTime;
        playerManager.interactTimeFill.fillAmount = 0f;
        while (Input.GetButton("Interact"))
        {
            timeOfInteraction -= Time.deltaTime;
            playerManager.interactTimeFill.fillAmount += Time.deltaTime / stealTime;

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
