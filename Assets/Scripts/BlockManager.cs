using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    //public int cubeQuantity; // Số lượng cube con trong block
    // public string[] color; // Màu của block
    public bool isUsed = false; // Trạng thái của block
                                // public int quantity; // Số lượng lớp trong block

    private void Awake()
    {

    }
    void Start()
    {
        if (GetCubeMatInSite() != null)
        {
            GetCubeMatInSite().transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            GetCubeMatInSite().gameObject.SetActive(true);
        }
        foreach (var cube in GetCubes())
        {
            cube.gameObject.SetActive(false); // Ẩn tất cả cube con khi khởi tạo block
        }
        GetCubeMatOutSite().transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        GetCubeMatOutSite().gameObject.SetActive(true); // Hiển thị cubeMatOutSite
    }
    //Trả về tất cả cube con
    public List<Transform> GetCubes()
    {
        List<Transform> cubes = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.tag != "cubeMatOut" && child.tag != "cubeMatIn")
            {
                cubes.Add(child);
            }

        }
        return cubes;
    }

    // Lấy ra tất cả các cube con lớp ngoài cùng
    public List<Transform> GetCubeOutSite()
    {
        List<Transform> cubes = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.tag != "cubeMatOut" && child.tag != "cubeMatIn")
            {
                CubeManager cubeManager = child.GetComponent<CubeManager>();
                if (cubeManager.status == "cubeOut")
                    cubes.Add(child);
            }

        }
        return cubes;
    }
    // Lấy ra tất cả các cube con lớp trong
    public List<Transform> GetCubeInSite()
    {
        List<Transform> cubes = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.tag != "cubeMatOut" && child.tag != "cubeMatIn")
            {
                CubeManager cubeManager = child.GetComponent<CubeManager>();
                if (cubeManager.status == "cubeIn")
                    cubes.Add(child);
            }
        }
        return cubes;
    }

    // Lấy GridCell mà mỗi cube đang chiếm
    public HashSet<GridCell> GetOccupiedCells()
    {
        HashSet<GridCell> cells = new HashSet<GridCell>();
        foreach (var cube in GetCubes())
        {
            if (cube.tag != "cubeMatOut" && cube.tag != "cubeMatIn")
            {
                GridCell cell = GridManager.Instance.GetClosestCell(cube.position);
                if (cell != null)
                {
                    cells.Add(cell);
                }
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
    // Lấy ra cubeMat ở ngoài
    public GameObject GetCubeMatOutSite()
    {
        GameObject cubeMat = null;
        foreach (Transform child in transform)
        {
            if (child.tag == "cubeMatOut")
            {
                cubeMat = child.gameObject;
            }

        }
        return cubeMat;
    }

    // Lấy ra cubeMat ở trong
    public GameObject GetCubeMatInSite()
    {
        GameObject cubeMat = null;
        foreach (Transform child in transform)
        {
            if (child.tag == "cubeMatIn")
            {
                cubeMat = child.gameObject;
            }

        }
        return cubeMat;
    }
    // Phá hủy block
    public void Explode()
    {
        Destroy(GetCubeMatOutSite());
        if (GetCubeMatInSite() != null)
        {
            GetCubeMatInSite().transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                GetCubeMatInSite().tag = "cubeMatOut";
            });

        }

        foreach (var cube in GetCubeOutSite())
        {
            cube.gameObject.SetActive(true);
            cube.DOScale(Vector3.one * 0.3f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                cube.DOMove(new Vector3(cube.transform.position.x, cube.transform.position.y + 2f, cube.transform.position.z), 0.1f).OnComplete(() =>
                {
                    cube.DOMove(new Vector3(-0.15f, 0f, 7f), 0.5f)
                    .SetEase(Ease.InBack).OnComplete(() =>
                    {
                        GameManager.Instance.score++; // Tăng điểm khi phá hủy block
                        Destroy(cube.gameObject);
                    });
                });
            });
        }

        foreach (var cube in GetCubeInSite())
        {
            CubeManager cubeManager = cube.GetComponent<CubeManager>();
            cubeManager.status = "cubeOut"; // cho cube trong thành cube ngoài 
            cube.DOScale(new Vector3(1f, 1f, 1f), 0.4f).SetEase(Ease.InBack);
        }
        foreach (var cell in GetOccupiedCells())
        {
            if (cell.layers.Count > 0)
            {
                cell.layers.Dequeue(); // Giảm số lượng layer trong ô
            }
        }
    }

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
    public Color GetColorOutSite()
    {
        Color col = Color.white;
        List<Transform> cubes = GetCubeOutSite();
        if (cubes.Count == 0) return col;
        col = cubes[0].GetComponent<Renderer>().material.color; // Lấy màu của cube ngoài cùng
        return col;

        //string colorOutSite = "";
        //if (quantity == 2)
        //{
        //    colorOutSite = color[0];
        //}
        //else if (quantity == 1)
        //{
        //    colorOutSite = color[1];
        //}
        //return colorOutSite;
    }
    // Giảm số lượng lớp trong block
}
