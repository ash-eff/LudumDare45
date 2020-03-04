using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public Grid floorGrid;
    public Grid ventGrid;
    public Grid fogGrid;
    public List<Vector2> walkableGrid;
    public Transform entrance;
    public Transform exit;
    public float peakRadius;
    public bool roomLoaded;
    public bool resetRoom = false;

    List<Vector3Int> allTilePos = new List<Vector3Int>();
    Tilemap fogTiles;

    public GameObject roomHolder;
    private Tilemap floorTileMap;
    public Color fogColor;
    public Color otherFogColor;

    private Dictionary<Vector2, bool> theGrid;

    public bool minimapActive;

    void Awake()
    {
        //roomHolder.SetActive(false);
        fogTiles = fogGrid.GetComponentInChildren<Tilemap>();
        fogGrid.gameObject.SetActive(true);
        fogColor = new Color(fogTiles.color.r, fogTiles.color.g, fogTiles.color.b, fogTiles.color.a);
        GetAllFogTilePositions();
        GetGridDictionary();
        transform.position = new Vector3(transform.position.x, transform.position.y, 5);
    }

    private void GetGridDictionary()
    {
        floorTileMap = floorGrid.GetComponentInChildren<Tilemap>();
        floorTileMap.CompressBounds();
        floorTileMap.origin = new Vector3Int(0,0,0);
        BoundsInt bounds = new BoundsInt(floorTileMap.origin, floorTileMap.size);
        Ash.Grid grid = new Ash.Grid(bounds.size.x * (int)floorGrid.cellSize.x, bounds.size.y * (int)floorGrid.cellSize.y, (int)transform.position.x, (int)transform.position.y, 1);
        theGrid = new Dictionary<Vector2, bool>();
        foreach (Vector2 pos in grid.GetGridPositions())
        {
            theGrid.Add(pos, CheckIfPositionWalkable(pos));
        }

        CreateWalkablePositionList(theGrid);
    }

    private void CreateWalkablePositionList(Dictionary<Vector2, bool> theGrid)
    {
        walkableGrid = new List<Vector2>();
        foreach(KeyValuePair<Vector2, bool> kvp in theGrid)
        {
            if(kvp.Value == true)
            {
                walkableGrid.Add(kvp.Key);
            }
        }
        roomLoaded = true;
    }

    private bool CheckIfPositionWalkable(Vector2 pos)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 15f, obstacleLayer);
        if (hit)
        {
            Debug.DrawRay(ray.origin, ray.direction * 15f, Color.red, 100f);
            return false;
        }

        RaycastHit2D secondHit = Physics2D.Raycast(ray.origin, ray.direction, 15f);
        if (!secondHit)
        {
            Debug.DrawRay(ray.origin, ray.direction * 15f, Color.yellow, 100f);
            return false;
        }

        Debug.DrawRay(ray.origin, ray.direction * 15f, Color.blue, 100f);
        return true;
    }

    public void SwapToVents(bool swap)
    {
        if (swap)
        {
            ventGrid.gameObject.SetActive(true);
        }
        else
        {
            ventGrid.gameObject.SetActive(false);
        }
    }

    private void GetAllFogTilePositions()
    {
        Tilemap fogTiles = fogGrid.GetComponentInChildren<Tilemap>();
        fogTiles.CompressBounds();
        fogTiles.origin = Vector3Int.zero;
        BoundsInt bounds = fogTiles.cellBounds;
        TileBase[] allTiles = fogTiles.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    fogTiles.SetTileFlags(new Vector3Int((int)x, (int)y, 0), TileFlags.None);
                    allTilePos.Add(new Vector3Int((int)x, (int)y, 0));
                }
            }
        }
    }

    public void PeakIntoRoom(Vector2 _localPeakPos)
    {
        resetRoom = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        foreach (Vector3Int pos in allTilePos)
        {
            float distance = (new Vector3Int((int)_localPeakPos.x, (int)_localPeakPos.y, 0) - pos).magnitude;
            if(distance <= peakRadius)
            {
                StartCoroutine(FadeTile(pos, distance / peakRadius));
            }
        }
    }

    IEnumerator FadeTile(Vector3Int pos, float dist)
    {
        Color A = new Color(1, 1, 1, 1);
        Color B = new Color(1, 1, 1, dist);
        float lerpTime = 1f;
        float currentLerpTime = 0;
        while (fogTiles.GetColor(pos) != B && !resetRoom)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            Color color = Color.Lerp(A, B, perc);
            fogTiles.SetColor(pos, color);

            yield return null;
        }
    }

    public void ResetRoom()
    {
        foreach (Vector3Int pos in allTilePos)
        {
            fogTiles.SetColor(pos, new Color(1,1,1,1));
            //StartCoroutine(ResetFogColor(pos));          
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        resetRoom = true;
    }

    IEnumerator ResetFogColor(Vector3Int pos)
    {
        Color A = new Color(1,1,1,fogTiles.GetColor(pos).a);
        Color B = new Color(1, 1, 1, 1);
        float lerpTime = 1f;
        float currentLerpTime = 0;
        while (fogTiles.GetColor(pos).a != 1)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            Color color = Color.Lerp(A, B, perc);
            fogTiles.SetColor(pos, color);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (walkableGrid != null)
        {
            foreach (Vector2 pos in walkableGrid)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(pos, .1f);
            }
        }
    }
}
