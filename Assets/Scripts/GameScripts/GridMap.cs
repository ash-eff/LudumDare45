﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    public Tilemap tm;
    public List<Vector3> walkableTiles = new List<Vector3>();
    public bool gridLoaded;

    private void Awake()
    {
        GetGridOfTilePositions();
    }

    public void GetGridOfTilePositions()
    {
        for (int x = tm.cellBounds.xMin; x < tm.cellBounds.xMax; x++)
        {
            for (int y = tm.cellBounds.yMin; y < tm.cellBounds.yMax; y++)
            {
                Vector3Int location = (new Vector3Int(x, y, 0));
                if (tm.HasTile(new Vector3Int(x, y, 0)))
                {
                    Vector3 offsetLocation = new Vector3(location.x + 0.5f, location.y + 0.5f, 0f);
                    walkableTiles.Add(offsetLocation);
                }
            }
        }

        gridLoaded = true;
    }

    private void OnDrawGizmos()
    {
        foreach(Vector3 walkable in walkableTiles)
        {
            Gizmos.DrawWireSphere(walkable, .1f);
        }
    }
}
