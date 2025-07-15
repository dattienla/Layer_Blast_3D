using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public int cubeQuantity; // Số lượng cube con trong block
    public Color color; // Màu của block

    private void Start()
    {
        color = GetColor(); // Lấy màu từ cube con đầu tiên
    }

    // Trả về tất cả cube con
    public List<Transform> GetCubes()
    {
        List<Transform> cubes = new List<Transform>();
        foreach (Transform child in transform)
        {
            cubes.Add(child);
        }
        return cubes;
    }

    // Lấy GridCell mà mỗi cube đang chiếm
    public List<GridCell> GetOccupiedCells()
    {
        List<GridCell> cells = new List<GridCell>();
        foreach (var cube in GetCubes())
        {
            GridCell cell = GridManager.Instance.GetClosestCell(cube.position);
            if (cell != null)
            {
                cells.Add(cell);
            }
        }
        return cells;
    }

    // Lấy ra tất cả hàng xóm của block
    public List<GridCell> GetNeighborOfBlock()
    {
        HashSet<GridCell> neighborsOfBlock = new HashSet<GridCell>();
        foreach (var cell in GetOccupiedCells())
        {
            foreach (GridCell neighbor in GridManager.Instance.GetNeighbors(cell))
            {
                GameObject neighborTop = neighbor.PeekTopLayer();
                if (neighborTop == null) continue;

                // Bỏ qua nếu là cube cùng block
                if (neighborTop.transform.IsChildOf(this.transform)) continue;
                neighborsOfBlock.Add(neighbor);
            }
        }
        return new List<GridCell>(neighborsOfBlock);
    }
    // Lấy ra màu của block (lấy từ cube con)
    public Color GetColor()
    {
        if (GetCubes().Count > 0)
        {
            Renderer renderer = GetCubes()[0].GetComponent<Renderer>();
            if (renderer != null)
            {
                return renderer.material.color;
            }
        }
        return Color.white; // Mặc định nếu không tìm thấy
    }
    // Phá hủy block
    public void Explode()
    {
        foreach (var cube in GetCubes())
        {
            cube.DOScale(Vector3.one * 0.3f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                cube.DOMove(new Vector3(-0.15f, 0f, 7f), 0.7f)
                    .SetEase(Ease.InBack).OnComplete(() =>
                {
                    GameManager.Instance.score++; // Tăng điểm khi phá hủy block
                    Destroy(cube.gameObject);
                });
            });
        }
        foreach (var cell in GetOccupiedCells())
        {
            if (cell.layers.Count > 0)
            {
               // GameManager.Instance.score++; // Tăng điểm khi phá hủy block
                cell.layers.Clear(); // Giảm số lượng layer trong ô
            }
        }
    }
}
