using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Node startNode, endNode;

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Queue<Node> queue = new Queue<Node>();
    bool isRunning = true;
    Node searchCenter;
    public List<Node> path = new List<Node>();

    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
        new Vector2Int(1,1),  new Vector2Int(-1, 1), new Vector2Int(-1,-1),  new Vector2Int(-1,1)};

    private void Awake()
    {
        LoadNodes();
    }

    public List<Node> GetPath(Node _startNode, Node _endNode)
    {
        startNode = _startNode;
        endNode = _endNode;
        ResetPath();

        if(path.Count == 0)
        {
            CalculatePath();
        }

        return path;
    }

    private void ResetPath()
    {
        searchCenter = null;
        startNode.isExplored = false;
        startNode.exploredFrom = null;
        endNode.isExplored = false;
        endNode.exploredFrom = null;
        path.Clear();
        
        foreach(KeyValuePair<Vector2Int, Node> node in grid)
        {
            node.Value.isExplored = false;
            node.Value.exploredFrom = null;
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
        SetAsPath(endNode);

        Node previous = endNode.exploredFrom;

        while(previous != startNode)
        {
            SetAsPath(previous);
            previous = previous.exploredFrom;
        }

        SetAsPath(startNode);
        path.Reverse();
    }

    private void SetAsPath(Node node)
    {
        path.Add(node);
    }

    private void BreadthFirstSearch()
    {
        queue.Enqueue(startNode);

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbors();
            searchCenter.isExplored = true;
        }
    }

    private void HaltIfEndFound()
    {
        if(searchCenter == endNode)
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

        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = searchCenter.GetGridPos() + direction;
            if (grid.ContainsKey(neighborCoordinates))
            {
                QueueNewNeighbors(neighborCoordinates);
            }
        }
    }

    private void QueueNewNeighbors(Vector2Int neighborCoordinates)
    {
        Node neighbor = grid[neighborCoordinates];
        if(neighbor.isExplored || queue.Contains(neighbor))
        {
            // do nothing
        }
        else
        {
            queue.Enqueue(neighbor);
            neighbor.exploredFrom = searchCenter;
        }
    }

    private void LoadNodes()
    {
        Node[] nodes = FindObjectsOfType<Node>();
        foreach (Node node in nodes)
        {
            var gridPos = node.GetGridPos();
            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Skipping overlapping node " + node + "at: " + node.GetGridPos().ToString());
            }
            else
            {
                grid.Add(gridPos, node);
            }
        }
    }
}
