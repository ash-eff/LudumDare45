using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour, IInteractable
{
    public Room thisRoom;
    public Room nextRoom;
    public RoomEntrance nextRoomEntrance;
    public GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        thisRoom = GetComponentInParent<Room>();
    }

    public void SwapRooms()
    {
        gameController.SwapRooms(nextRoom, nextRoomEntrance);
    }

    public void PeakIntoRoom()
    {
        Vector2 dir = nextRoomEntrance.transform.position - transform.position;

        // check if this room exits down
        if (dir.y < 0)
        {
            thisRoom.ResetRoom();
            nextRoom.PeakIntoRoom();
        }

        gameController.PeakIntoRoom(nextRoomEntrance.transform.position, nextRoom);
    }

    public void ResetPeak()
    {
        Vector2 dir = nextRoomEntrance.transform.position - transform.position;

        // check is this room exits down
        if (dir.y < 0)
        {
            thisRoom.SelectRoom();
            nextRoom.ResetRoom();
        }
        
        gameController.StopPeaking();
    }

    public void Interact()
    {
    }

    public string BeingTouched()
    {
        return "Press E to enter next area. \n Press R to peak into next area.";

    }
}
