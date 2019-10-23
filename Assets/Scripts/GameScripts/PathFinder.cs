using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startPosition, endPosition;

    public Tilemap tileMap;
    public Dictionary<Vector2Int, MapPointInfo> map = new Dictionary<Vector2Int, MapPointInfo>();
    public Queue<Vector2Int> queue = new Queue<Vector2Int>();
    bool isRunning = true;
    Vector2Int searchCenter;
    public List<Vector2Int> path = new List<Vector2Int>();

    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left, };
    //new Vector2Int(1,1),  new Vector2Int(-1, 1), new Vector2Int(-1,-1),  new Vector2Int(-1,1)};

    GridMap grid;

    private void Start()
    {
        grid = FindObjectOfType<GridMap>();
        GenerateMap();
    }

    public List<Vector2Int> GetPath(Vector2Int _startPosition, Vector2Int _endPosition)
    {
        startPosition = _startPosition;
        endPosition = _endPosition;
        //ResetPath();

        if(path.Count == 0)
        {
            CalculatePath();
        }

        return path;
    }

    private void ResetPath()
    {
        path.Clear();
        
        foreach(var item in map)
        {
            item.Value.ResetValues();
        }

        queue.Clear();
        isRunning = true;
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
            Debug.Log(map[previous]);
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

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbors();
        }
    }

    private void HaltIfEndFound()
    {       
        if(searchCenter == endPosition)
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
                Debug.Log((Vector2Int)pos);
            }
        }
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

        public void ResetValues()
        {
            exploredFrom = Vector2Int.zero;
            hasBeenExplored = false;
        }
    }

}
