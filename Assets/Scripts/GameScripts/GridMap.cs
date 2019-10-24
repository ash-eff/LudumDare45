using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public List<Vector3Int> theGrid = new List<Vector3Int>();
    public bool isReady;

    private void Awake()
    {
        BuildGrid();
    }

    public void BuildGrid()
    {
        for(int x = 0; x < gridWidth; x++)
        {
            for(int y = 0; y < gridHeight; y++)
            {
                theGrid.Add(new Vector3Int(x, y, 0));
            }
        }

        isReady = true;
    }
}
