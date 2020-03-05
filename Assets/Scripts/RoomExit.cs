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
        gameController.currentRoom.fogGrid.gameObject.SetActive(true);
        //nextRoom.PeakIntoRoom(nextRoomEntrance.transform.localPosition);
    }

    public void ResetPeak()
    {
        gameController.currentRoom.fogGrid.gameObject.SetActive(false);
        nextRoom.ResetRoom();
    }

    public void Interact()
    {
    }

    public string BeingTouched()
    {
        Debug.Log(this.transform.name);
        return "Press E to enter next area. \n Press R to peak into next area.";

    }
}
