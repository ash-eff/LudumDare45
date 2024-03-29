﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    public Room room;

    public bool isGeneratingMap = true;
    public bool mapComplete;

    public List<Vector3> path = new List<Vector3>();
    public Dictionary<Vector3, MapPointInfo> map = new Dictionary<Vector3, MapPointInfo>();

    private Queue<Vector3> queue = new Queue<Vector3>();
    private List<Vector3> searchedItems = new List<Vector3>();
    private Vector3[] directions = { Vector3.up, // up
                                Vector3.right, // right
                                -Vector3.up, // down
                                -Vector3.right, // left
                                new Vector3(1, 1, 0), // up-right
                                new Vector3(1, -1, 0), // right-down
                                new Vector3(-1, -1, 0), // down-left
                                new Vector3(-1, 1, 0) }; // left-up

    private Vector3 startPosition, endPosition;
    private Vector3 searchCenter;

    private bool isRunning = true;

    private void Awake()
    {
        room = GetComponentInParent<Room>();
    }

    public List<Vector3> GetPath(Vector3 _startPosition, Vector3 _endPosition)
    {
        startPosition = _startPosition;
        endPosition = _endPosition;
        path.Clear();

        foreach (Vector3 v in searchedItems)
        {
            map[v] = new MapPointInfo(Vector3.zero, false);
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
        Vector3 previous = map[endPosition].GetExploredFrom;

        while (previous != startPosition)
        {
            SetAsPath(previous);
            previous = map[previous].GetExploredFrom;
        }

        SetAsPath(startPosition);
        path.Reverse();
    }

    private void SetAsPath(Vector3 pos)
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

        foreach (Vector3 direction in directions)
        { 
            Vector3 neighborCoordinates = searchCenter + direction;
            if (map.ContainsKey(neighborCoordinates))
            {
                QueueNewNeighbors(neighborCoordinates);
            }
        }
    }

    private void QueueNewNeighbors(Vector3 neighborCoordinates)
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

    public void GetAGridMap()
    {
        foreach (Vector3 pos in room.walkableGrid)
        {
            MapPointInfo mapPointInfo = new MapPointInfo(pos, false);
            map.Add(pos, mapPointInfo);
        }

        isGeneratingMap = false;
    }

    public struct MapPointInfo
    {
        public Vector3 exploredFrom;
        public bool hasBeenExplored;

        public MapPointInfo(Vector3 _exploredFrom, bool _hasBeenExplored)
        {
            exploredFrom = _exploredFrom;
            hasBeenExplored = _hasBeenExplored;
        }

        public bool GetHasBeenExplored
        {
            get { return hasBeenExplored; }
        }

        public Vector3 GetExploredFrom
        {
            get { return exploredFrom; }
        }
    }

    private void OnDrawGizmos()
    {
        if(path.Count != 0)
        {
            foreach (Vector3 v in path)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(new Vector3(v.x, v.y, 0), .1f);
            }
        }
    }
}
