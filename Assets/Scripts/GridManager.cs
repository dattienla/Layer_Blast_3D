using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int width;
    public int height;
    public GridCell[,] grid;

    private void Awake()
    {
        width = 5;
        height = 6;
        Instance = this;
        grid = new GridCell[10, 10];
        GridCell[] allTiles = FindObjectsOfType<GridCell>();
        foreach (var cell in allTiles)
        {
            if (IsInsideGrid(cell.x, cell.y))
            {
                grid[cell.x, cell.y] = cell;
            }
        }
    }
    // lấy ra cell tại toạ độ nào đó trong grid
    public GridCell GetCellInPos(Vector2Int pos)
    {
        GridCell cellInPos = null;

        foreach (var cell in grid)
        {
            if (cell == null) continue;
            if (pos.x == cell.x | pos.y == cell.y) cellInPos = cell;
        }
        return cellInPos;
    }
    /// <summary>
    /// Trả về ô gần nhất với 1 vị trí trong thế giới (dùng để snap)
    /// </summary>
    public GridCell GetClosestCell(Vector3 worldPos)
    {
        float minDist = 1.2f; // khoảng cách tối thiểu để snap
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

    // lấy ra tất cả ô hàng xóm của 1 ô cụ thể
    public List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new List<GridCell>();

        // Các hướng: phải, trái, trên, dưới
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(1, 0),  // phải
        new Vector2Int(-1, 0), // trái
        new Vector2Int(0, 1),  // trên
        new Vector2Int(0, -1), // dưới
        };

        foreach (Vector2Int dir in directions)
        {
            int nx = cell.x + dir.x;
            int ny = cell.y + dir.y;

            if (IsInsideGrid(nx, ny))
            {
                GridCell neighbor = grid[nx, ny];
                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    // check giới hạn biên của lưới
    private bool IsInsideGrid(int x, int y)
    {
        return x > 0 && x <= width && y > 0 && y <= height;
    }
}
