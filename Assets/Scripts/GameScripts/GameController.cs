using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Ash.PlayerController;

public class GameController : MonoBehaviour
{
    public Room startingRoom;
    public Room currentRoom;
    public Room lastRoom;
    public RoomEntrance roomEntrance;
    public GameObject[] minimaps;
    public Image fadeImage;
    private PlayerController player;
    public bool transferRoom;
    public Transform startPoint;
    public Tilemap fogTiles;
    public Tilemap roomFloorTiles;
    public GameObject aPrefab;
    public GameObject aPrefab2;

    List<Vector3Int> allFogTilePos = new List<Vector3Int>();
    List<Vector3Int> currentRoomTiles = new List<Vector3Int>();
    List<Vector3Int> peakRoomTiles = new List<Vector3Int>();
    public float peakRadius;
    public int layerMod = 2;

    private void Awake()
    {
        fogTiles.gameObject.SetActive(true);
        currentRoom = startingRoom;
        player = FindObjectOfType<PlayerController>();
        player.transform.position = startPoint.position;
    }

    private void Start()
    {
        currentRoom.fogGrid.gameObject.SetActive(false);
        allFogTilePos = GetAllTilePositions(fogTiles);
        currentRoom.SelectRoom();
        ShowRoom(currentRoom);
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

    public void SwapRooms(Room toRoom, RoomEntrance toRoomEntrance)
    {
        lastRoom = currentRoom;
        currentRoom = toRoom;
        roomEntrance = toRoomEntrance;
        StartCoroutine(RoomSwapFade());
    }

    public void PeakIntoRoom(Vector2 _peakPos, Room _peakRoom)
    {
        // can clean this up and create more helper functions
        ResetFog();
        List<Vector3Int> peakRoom = GetAllTilePositions(_peakRoom.fogGrid.GetComponentInChildren<Tilemap>());
        Dictionary<Vector3Int, float> peakRadiusDict = new Dictionary<Vector3Int, float>();

        foreach(Vector3Int pos in peakRoom)
        {
            float distance = (new Vector3Int((int)_peakPos.x, (int)_peakPos.y, 0) - pos).magnitude;
            if (distance <= peakRadius)
            {
                peakRadiusDict.Add(pos, distance/peakRadius);
            }
        }

        foreach (Vector3Int pos in currentRoomTiles)
        {
            if (peakRadiusDict.ContainsKey(pos))
            {
                // skip
            }
            else
            {
                float distance = (new Vector3Int((int)_peakPos.x, (int)_peakPos.y, 0) - pos).magnitude;
                if (distance <= peakRadius)
                {
                    peakRadiusDict.Add(pos, distance / peakRadius);
                }
            }
        }

        foreach(KeyValuePair<Vector3Int,float> kvp in peakRadiusDict)
        {
            FadeTile(kvp.Key, fogTiles.GetColor(kvp.Key).a, kvp.Value);
        }
    }

    public void StopPeaking()
    {
        ResetFog();
        ShowRoom(currentRoom);
    }

    private List<Vector3Int> GetAllTilePositions(Tilemap _tilemap)
    {
        Room tilemapRoom = _tilemap.GetComponentInParent<Room>();
        _tilemap.CompressBounds();
        _tilemap.origin = Vector3Int.zero;
        BoundsInt bounds = _tilemap.cellBounds;
        TileBase[] allTiles = _tilemap.GetTilesBlock(bounds);
        List<Vector3Int> allTilePos = new List<Vector3Int>();

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    if(tilemapRoom == null)
                    {
                        allTilePos.Add(new Vector3Int((int)x, (int)y, 0));
                        _tilemap.SetTileFlags(new Vector3Int((int)x, (int)y, 0), TileFlags.None);
                    }
                    else
                    {
                        Vector3Int roomOffsetPos = new Vector3Int((int)x + (int)tilemapRoom.transform.position.x, (int)y + (int)tilemapRoom.transform.position.y, 0);
                        allTilePos.Add(roomOffsetPos);
                        _tilemap.SetTileFlags(roomOffsetPos, TileFlags.None);
                    }
                }
            }
        }

        return allTilePos;
    }

    private void ShowRoom(Room _room)
    {
        roomFloorTiles = _room.fogGrid.GetComponentInChildren<Tilemap>();
        roomFloorTiles.CompressBounds();
        roomFloorTiles.origin = Vector3Int.zero;
        BoundsInt bounds = roomFloorTiles.cellBounds;
        TileBase[] allTiles = roomFloorTiles.GetTilesBlock(bounds);
        currentRoomTiles = GetAllTilePositions(roomFloorTiles);

        foreach (Vector3Int pos in currentRoomTiles)
        {
            //Vector3Int newPos = new Vector3Int(pos.x + (int)_room.transform.position.x, pos.y + (int)_room.transform.position.y, 0);
            FadeTile(pos, fogTiles.GetColor(pos).a, 0);
        }
    }

    private void ResetFog()
    {
        foreach (Vector3Int pos in allFogTilePos)
        {
            FadeTile(pos, fogTiles.GetColor(pos).a, 1);
        }
    }

    void FadeTile(Vector3Int pos, float startVal, float endVal)
    {
        Color A = new Color(1, 1, 1, startVal);
        Color B = new Color(1, 1, 1, endVal);
        float lerpTime = .1f;
        float currentLerpTime = 0;
        if (fogTiles.GetColor(pos) != B)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            //Color color = Color.Lerp(A, B, perc);
            Color color = new Color(1f, 1f, 1f, endVal);
            fogTiles.SetColor(pos, color);

            //yield return null;
        }
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
        ResetFog();
        lastRoom.ResetRoom();
        currentRoom.SelectRoom();
        //yield return new WaitForSeconds(.5f);

        ShowRoom(currentRoom);
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
