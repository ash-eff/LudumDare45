using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Ash
{
    public class Grid
    {
        private int width;
        private int height;
        private int widthOffset;
        private int heightOffset;
        private float cellSize;
        private int[,] gridArray;
        private List<Vector2> grid;

        public Grid(int width, int height, int widthOffset, int heightOffset, float cellSize)
        {
            grid = new List<Vector2>();
            this.width = width;
            this.height = height;
            this.widthOffset = widthOffset;
            this.heightOffset = widthOffset;
            this.cellSize = cellSize;

            gridArray = new int[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    grid.Add(GetWorldPosition(x + widthOffset + (cellSize / 2), y + heightOffset + (cellSize / 2)));
                }
            }
        }

        private Vector3 GetWorldPosition(float x, float y)
        {
            return new Vector3(x, y) * cellSize;
        }

        public List<Vector2> GetGridPositions()
        {
            return grid;
        }
    }
}

