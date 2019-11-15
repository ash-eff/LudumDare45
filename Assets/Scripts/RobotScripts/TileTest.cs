using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public Tilemap tm;
    //public TileBase[] allTiles;
    //BoundsInt bounds;
    public List<Vector3> tilePos = new List<Vector3>();

    private void Start()
    {
        //bounds = tm.cellBounds;
        //allTiles = tm.GetTilesBlock(bounds);
        GetTilePositions();
    }

    void GetTilePositions()
    {
        Debug.Log(tm.cellBounds);
        for(int x = tm.cellBounds.xMin; x < tm.cellBounds.xMax; x++)
        {
            for(int y = tm.cellBounds.yMin; y < tm.cellBounds.yMax; y++)
            {
                Vector3Int location = (new Vector3Int(x, y, 0));
                if (tm.HasTile(location))
                {
                    Debug.Log("Tile At: " + location);
                    tilePos.Add(location);
                }
            }
        }
    }
}
