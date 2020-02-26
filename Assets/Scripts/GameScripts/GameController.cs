using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ash.PlayerController;

public class GameController : MonoBehaviour
{
    public Room[] availableRooms;
    public Room currentRoom;
    public GameObject[] minimaps;
    private PlayerController player;
    public bool transferRoom;

    private void Awake()
    {
        currentRoom = availableRooms[0];
        StartCoroutine(currentRoom.ChangeFogAlpha(0));
        player = FindObjectOfType<PlayerController>();
        player.transform.position = currentRoom.entrance.position;
    }

    private void Update()
    {
        if (transferRoom)
        {
            transferRoom = false;
            SwapRooms();
        }
    }

    public void ToggleMinimap()
    {
        if (!currentRoom.minimapActive)
        {
            currentRoom.minimapActive = true;
            foreach (GameObject map in minimaps)
            {
                map.SetActive(true);
            }
        }
    }

    void SwapRooms()
    {
        StartCoroutine(currentRoom.ChangeFogAlpha(1));
        currentRoom = availableRooms[1];
        player.transform.position = currentRoom.entrance.position;
        StartCoroutine(currentRoom.ChangeFogAlpha(0));
    }
}
