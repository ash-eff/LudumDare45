using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    Vector2Int startPosition, endPosition;

    public Tilemap tileMap;
    public Dictionary<Vector2Int, MapPointInfo> map = new Dictionary<Vector2Int, MapPointInfo>();
    public Queue<Vector2Int> queue = new Queue<Vector2Int>();
    bool isRunning = true;
    public bool mapComplete;
    Vector2Int searchCenter;
    public List<Vector2Int> path = new List<Vector2Int>();
    List<Vector2Int> searchedItems = new List<Vector2Int>();
    public bool isGeneratingMap = true;

    GridMap grid;

    Vector2Int[] directions = { Vector2Int.up, 
                                Vector2Int.right,
                                Vector2Int.down, 
                                Vector2Int.left,
                                new Vector2Int(1, 1), // up-right
                                new Vector2Int(1, -1), // right-down
                                new Vector2Int(-1, -1), //down-left
                                new Vector2Int(-1, 1) }; // left-up

    private void Start()
    {
        grid = FindObjectOfType<GridMap>();
        GenerateMap();
    }

    public List<Vector2Int> GetPath(Vector2Int _startPosition, Vector2Int _endPosition)
    {
        startPosition = _startPosition;
        endPosition = _endPosition;
        path.Clear();

        foreach (Vector2Int v in searchedItems)
        {
            map[v] = new MapPointInfo(Vector2Int.zero, false);
        }

        searchedItems.Clear();
        queue.Clear();

        isRunning = true;

        if (path.Count == 0)
        {
            CalculatePath();
        }

        return path;
    }

    private void CalculatePath()
    {
        BreadthFirstSearch();
        CreatePath();
    }

    private void CreatePath()
    {
        SetAsPath(endPosition);
        Vector2Int previous = map[endPosition].GetExploredFrom;

        while (previous != startPosition)
        {
            SetAsPath(previous);
            previous = map[previous].GetExploredFrom;
        }

        SetAsPath(startPosition);
        path.Reverse();
    }

    private void SetAsPath(Vector2Int pos)
    {
        path.Add(pos);
    }

    private void BreadthFirstSearch()
    {
        queue.Enqueue(startPosition);
        searchedItems.Add(startPosition);

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbors();
        }
    }

    private void HaltIfEndFound()
    {
        if (searchCenter == endPosition)
        {
            isRunning = false;
        }
    }

    private void ExploreNeighbors()
    {
        if (!isRunning)
        {
            return;
        }

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = searchCenter + direction;

            if (map.ContainsKey(neighborCoordinates))
            {
                QueueNewNeighbors(neighborCoordinates);
            }
        }
    }

    private void QueueNewNeighbors(Vector2Int neighborCoordinates)
    {
        MapPointInfo info = map[neighborCoordinates];
        if(info.GetHasBeenExplored == true || queue.Contains(neighborCoordinates))
        {
            // skip
        }
        else
        {
            queue.Enqueue(neighborCoordinates);
            searchedItems.Add(neighborCoordinates);
            map[neighborCoordinates] = new MapPointInfo(searchCenter, true);
        }
    }

    private void GenerateMap()
    {
        foreach (Vector3Int pos in grid.theGrid)
        {
            if (GetTile(pos) != null)
            {
                MapPointInfo mapPointInfo = new MapPointInfo((Vector2Int)pos, false);
                map.Add((Vector2Int)pos, mapPointInfo);
            }
        }

        isGeneratingMap = false;
    }

    public TileBase GetTile(Vector3Int atLocation)
    {
        TileBase tile = tileMap.GetTile(atLocation);
        return tile;
    }

    public struct MapPointInfo
    {
        public Vector2Int exploredFrom;
        public bool hasBeenExplored;

        public MapPointInfo(Vector2Int _exploredFrom, bool _hasBeenExplored)
        {
            exploredFrom = _exploredFrom;
            hasBeenExplored = _hasBeenExplored;
        }

        public bool GetHasBeenExplored
        {
            get { return hasBeenExplored; }
        }

        public Vector2Int GetExploredFrom
        {
            get { return exploredFrom; }
        }
    }

    private void OnDrawGizmos()
    {
        if(path.Count != 0)
        {
            foreach (Vector2Int v in path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(new Vector3(v.x, v.y, 0), .2f);
            }
        }
    }
}
