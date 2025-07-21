using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    //public int cubeQuantity; // Số lượng cube con trong block
    public string[] color; // Màu của block
    public bool isUsed = false; // Trạng thái của block
    public int quantity; // Số lượng lớp trong block

    private void Awake()
    {

    }
    // Trả về tất cả cube con
    //public List<Transform> GetCubes()
    //{
    //    List<Transform> cubes = new List<Transform>();
    //    foreach (Transform child in transform)
    //    {
    //        cubes.Add(child);
    //    }
    //    return cubes;
    //}

    // Lấy ra tất cả các cube con lớp ngoài cùng
    public List<Transform> GetCubeOutSite()
    {
        List<Transform> cubes = new List<Transform>();
        foreach (Transform child in transform)
        {
            CubeManager cubeManager = child.GetComponent<CubeManager>();
            if (cubeManager.status == "cubeOut")
                cubes.Add(child);
        }
        return cubes;
    }
    // Lấy ra tất cả các cube con lớp trong
    public List<Transform> GetCubeInSite()
    {
        List<Transform> cubes = new List<Transform>();
        foreach (Transform child in transform)
        {
            CubeManager cubeManager = child.GetComponent<CubeManager>();
            if (cubeManager.status == "cubeIn")
                cubes.Add(child);
        }
        return cubes;
    }

    // Lấy GridCell mà mỗi cube đang chiếm
    public HashSet<GridCell> GetOccupiedCells()
    {
        HashSet<GridCell> cells = new HashSet<GridCell>();
        foreach (var cube in GetCubeOutSite())
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
        foreach (var cube in GetCubeOutSite())
        {
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
        if (quantity == 2)
        {
            foreach (var cube in GetCubeInSite())
            {
                CubeManager cubeManager = cube.GetComponent<CubeManager>();
                cubeManager.status = "cubeOut"; // cho cube trong thành cube ngoài 
                cube.DOScale(new Vector3(1f, 1f, 1f), 0.4f).SetEase(Ease.InBack);
            }
            if (color[0] == color[1]) quantity -= 2;
            else quantity--;
        }
        foreach (var cell in GetOccupiedCells())
        {
            if (cell.layers.Count > 0)
            {
                cell.layers.Pop(); // Giảm số lượng layer trong ô
            }
        }
    }
    //void ExplodeInSite()
    //{
    //    foreach (var cube in GetCubeInSite())
    //    {
    //        cube.DOScale(Vector3.one * 0.3f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
    //        {
    //            cube.DOMove(new Vector3(cube.transform.position.x, cube.transform.position.y + 0.1f, cube.transform.position.z), 0.1f).OnComplete(() =>
    //            {
    //                cube.DOMove(new Vector3(-0.15f, 0f, 7f), 0.7f)
    //                .SetEase(Ease.InBack).OnComplete(() =>
    //                {
    //                    GameManager.Instance.score++; // Tăng điểm khi phá hủy block
    //                    Destroy(cube.gameObject);
    //                });
    //            });
    //        });
    //    }
    //    foreach (var cell in GetOccupiedCells())
    //    {
    //        if (cell.layers.Count > 0)
    //        {
    //            cell.layers.Pop(); // Giảm số lượng layer trong ô
    //        }
    //    }
    //}

    // Lấy ra toạ độ tương đối của các cube con 
    public List<Vector2Int> GetPositionOfCubes()
    {
        List<Vector2Int> offsets = new List<Vector2Int>();
        foreach (Transform cube in GetCubeOutSite())
        {
            Vector2Int offset = new Vector2Int(
                Mathf.Abs(Mathf.RoundToInt(cube.localPosition.z)),
                Mathf.Abs(Mathf.RoundToInt(cube.localPosition.x))
            );
            offsets.Add(offset);
        }
        return offsets;
    }

    // Lấy ra màu của block
    public string GetColorOutSite()
    {
        string colorOutSite = "";
        if (quantity == 2)
        {
            colorOutSite = color[0];
        }
        else if (quantity == 1)
        {
            colorOutSite = color[1];
        }
        return colorOutSite;
    }
    // Giảm số lượng lớp trong block
    public void RemoveQuantity()
    {
        if (quantity > 0)
            quantity--;
    }
}
