using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int width = 6;
    public int height = 5;
    public GridCell[,] grid;

    private void Awake()
    {
        Instance = this;
        grid = new GridCell[10, 10];
    }
    private void Start()
    {
        GridCell[] allTiles = FindObjectsOfType<GridCell>();
        foreach (var tile in allTiles)
        {
            if (tile.x > 0 && tile.x <= width && tile.y > 0 && tile.y <= height)
            {
                grid[tile.x, tile.y] = tile;
            }
        }
    }
    /// <summary>
    /// Trả về ô gần nhất với 1 vị trí trong thế giới (dùng để snap)
    /// </summary>
    public GridCell GetClosestCell(Vector3 worldPos)
    {
        float minDist = float.MaxValue;
        GridCell closest = null;

        foreach (var cell in grid)
        {
            if (cell == null) continue;

            float dist = Vector3.Distance(worldPos, cell.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = cell;
            }
        }

        return closest;
    }
}
