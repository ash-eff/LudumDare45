using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    public bool isReady;
    public Tilemap tm;
    public List<Vector3> walkableTiles = new List<Vector3>();

    private void Awake()
    {
        GetGridOfTilePositions();
    }

    void GetGridOfTilePositions()
    {
        for (int x = tm.cellBounds.xMin; x < tm.cellBounds.xMax; x++)
        {
            for (int y = tm.cellBounds.yMin; y < tm.cellBounds.yMax; y++)
            {
                Vector3Int location = (new Vector3Int(x, 0, y));
                if (tm.HasTile(new Vector3Int(x, y, 0)))
                {
                    Vector3 offsetLocation = new Vector3(location.x + 0.5f, 0f, location.z + 0.5f);
                    walkableTiles.Add(offsetLocation);
                }
            }
        }
        isReady = true;
    }
}
