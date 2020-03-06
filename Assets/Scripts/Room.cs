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
    public Renderer[] rends;

    public bool roomLoaded;
    public GameObject exits;
    public GameObject shadows;

    private GameController gameController;
    private Tilemap floorTileMap;
    private List<Vector3Int> allTilePos = new List<Vector3Int>();
    private Tilemap fogTiles;

    public GameObject roomHolder;

    private Dictionary<Vector2, bool> theGrid;

    public bool minimapActive;

    void Awake()
    {
        ventGrid.gameObject.SetActive(true);
        shadows.SetActive(true);
        gameController = FindObjectOfType<GameController>();
        rends = GetComponentsInChildren<Renderer>();
        ventGrid.gameObject.SetActive(false);
        GetGridDictionary();
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

    public void SelectRoom()
    {
        foreach(Renderer rend in rends)
        {
            rend.sortingOrder += gameController.layerMod;
        }

        exits.gameObject.SetActive(true);
    }

    public void ResetRoom()
    {
        foreach (Renderer rend in rends)
        {
            rend.sortingOrder -= gameController.layerMod;
        }

        exits.gameObject.SetActive(false);
    }

    public void PeakIntoRoom()
    {
        foreach (Renderer rend in rends)
        {
            rend.sortingOrder += gameController.layerMod;
        }
        exits.gameObject.SetActive(true);
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
