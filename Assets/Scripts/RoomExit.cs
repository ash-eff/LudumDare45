using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public Room exitOf;
    public Room exitTo;
    public GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        exitOf = GetComponentInParent<Room>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameController.SwapRooms(exitTo);
        }
    }
}
