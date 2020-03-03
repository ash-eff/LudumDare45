using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour, IInteractable
{
    public Room thisRoom;
    public Room nextRoom;
    public RoomEntrance nextRoomEntrance;
    public GameController gameController;
    public SpriteMask sprMask;
    public float peakTime;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        thisRoom = GetComponentInParent<Room>();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        gameController.SwapRooms(nextRoom, nextRoomEntrance);
    //    }
    //}

    public void SwapRooms()
    {
        sprMask.gameObject.SetActive(false);
        gameController.SwapRooms(nextRoom, nextRoomEntrance);
    }

    public IEnumerator PeakIntoRoom()
    {
        sprMask.gameObject.SetActive(true);
        yield return new WaitForSeconds(peakTime);
        sprMask.gameObject.SetActive(false);
    }

    public void Interact()
    {
    }

    public string BeingTouched()
    {
        return "Press E to enter next area. \n Press R to peak into next area.";
    }
}
