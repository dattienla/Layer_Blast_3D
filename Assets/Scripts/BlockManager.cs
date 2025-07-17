using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public int cubeQuantity; // Số lượng cube con trong block
    public string color; // Màu của block
    public bool isUsed = false; // Trạng thái của block



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
    // Phá hủy block
    public void Explode()
    {
        //Debug.Log("sda");
        foreach (var cube in GetCubes())
        {
            Debug.Log("sda");
            Debug.Log("diem " + GameManager.Instance.score);
            cube.DOScale(Vector3.one * 0.3f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                cube.DOMove(new Vector3(cube.transform.position.x, cube.transform.position.y + 0.1f, cube.transform.position.z), 0.1f).OnComplete(() =>
                {
                    cube.DOMove(new Vector3(-0.15f, 0f, 7f), 0.7f)
                    .SetEase(Ease.InBack).OnComplete(() =>
                    {
                        GameManager.Instance.score++; // Tăng điểm khi phá hủy block
                        Destroy(cube.gameObject);
                    });
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

    // Lấy ra toạ độ tương đối của các cube con 
    public List<Vector2Int> GetPositionOfCubes()
    {
        List<Vector2Int> offsets = new List<Vector2Int>();
        foreach (Transform cube in GetCubes())
        {
            Vector2Int offset = new Vector2Int(
                Mathf.Abs(Mathf.RoundToInt(cube.localPosition.z)),
                Mathf.Abs(Mathf.RoundToInt(cube.localPosition.x))
            );
            offsets.Add(offset);
        }
        return offsets;
    }

}
