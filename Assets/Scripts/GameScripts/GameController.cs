using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ash.PlayerController;

public class GameController : MonoBehaviour
{
    public Room startingRoom;
    public Room currentRoom;
    public Room lastRoom;
    public GameObject[] minimaps;
    public Image fadeImage;
    private PlayerController player;
    public bool transferRoom;
    public Transform startPoint;

    private void Awake()
    {
        currentRoom = startingRoom;
        player = FindObjectOfType<PlayerController>();
        player.transform.position = startPoint.position;
    }

    private void Start()
    {
        currentRoom.fogGrid.gameObject.SetActive(false);
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

    public void SwapRooms(Room toRoom)
    {
        lastRoom = currentRoom;
        currentRoom = toRoom;
        StartCoroutine(RoomSwapFade());
    }

    IEnumerator RoomSwapFade()
    {
        player.stateMachine.ChangeState(WaitState.Instance);
        Color A = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        Color B = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
        float lerpTime = .5f;
        float currentLerpTime = 0;
        while (fadeImage.color != B)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            fadeImage.color = Color.Lerp(A, B, perc);

            yield return null;
        }

        player.stateMachine.ChangeState(RoomSwapState.Instance);
        lastRoom.fogGrid.gameObject.SetActive(true);
        currentRoom.fogGrid.gameObject.SetActive(false);
        currentLerpTime = 0;
        while(fadeImage.color != A)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            fadeImage.color = Color.Lerp(B, A, perc);

            yield return null;
        }

        player.stateMachine.ChangeState(BaseState.Instance);
    }
}
