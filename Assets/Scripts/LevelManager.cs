using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject level1;
    Vector2Int head = new Vector2Int(1, 1); // vector của GridManager
    List<GameObject> allBlockOutPref = new List<GameObject>();
    List<GameObject> allBlockInPref = new List<GameObject>();
    List<GameObject> allBlockMat = new List<GameObject>();
    private void Start()
    {
        transform.position = new Vector3(-2.5f, 0f, -15f);
        // Level1();
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutBack);
        Level();
    }
    void Level()
    {
        List<GridCell> targetCells = new List<GridCell>();
        for (int i = 0; i < level1.transform.childCount; i++)
        {
            for (int j = 0; j < level1.transform.GetChild(i).transform.childCount; j++)
            {
                GameObject obj = level1.transform.GetChild(i).GetChild(j).gameObject;
                if (obj.tag == "cubeMatOut" || obj.tag == "cubeMatIn")
                {
                    allBlockMat.Add(obj);
                    continue;
                }
                if (obj.GetComponent<CubeManager>().status == "cubeOut") allBlockOutPref.Add(obj);
                else if (obj.GetComponent<CubeManager>().status == "cubeIn") allBlockInPref.Add(obj);
            }
        }
        foreach (GameObject obj in allBlockOutPref)
        {
            Vector2Int pos = new Vector2Int(Mathf.Abs((int)obj.transform.localPosition.z), Mathf.Abs((int)obj.transform.localPosition.x));
            Vector2Int posCell = pos + head; // tính toán vị trí của ô trong grid
            GridCell cell = GridManager.Instance.grid[posCell.x, posCell.y]; // lấy cell tương ứng với toạ độ
            cell.layers.Enqueue(obj); // Thêm cube vào cell

            // Debug.Log(cell.name + " đã add " + obj.name);
            obj.transform.DOMove(cell.transform.position, 1f).SetEase(Ease.InBack).OnComplete(() =>
            {
                obj.transform.position = cell.transform.position;
            });
            BlockManager cube_BlockManger = obj.transform.parent.GetComponent<BlockManager>();
            cube_BlockManger.isUsed = true; // Đánh dấu block đã được sử dụng
        }
        foreach (GameObject obj in allBlockInPref)
        {
            Vector2Int pos = new Vector2Int(Mathf.Abs((int)obj.transform.localPosition.z), Mathf.Abs((int)obj.transform.localPosition.x));
            Vector2Int posCell = pos + head; // tính toán vị trí của ô trong grid
            GridCell cell = GridManager.Instance.grid[posCell.x, posCell.y]; // lấy cell tương ứng với toạ độ
            cell.layers.Enqueue(obj); // Thêm cube vào cell

            // Debug.Log(cell.name + " đã add " + obj.name);
            obj.transform.DOMove(cell.transform.position, 1f).SetEase(Ease.InBack).OnComplete(() =>
            {
                obj.transform.position = cell.transform.position;
            });
            BlockManager cube_BlockManger = obj.transform.parent.GetComponent<BlockManager>();
            cube_BlockManger.isUsed = true; // Đánh dấu block đã được sử dụng
        }
        foreach (GameObject obj in allBlockMat)
        {
            GameObject parent = obj.transform.parent.gameObject;
            List<GameObject> objWithParent = new List<GameObject>();
            foreach (GameObject op in allBlockOutPref)
            {
                if (op.transform.parent == obj.transform.parent)
                {
                    objWithParent.Add(op);
                }
            }
            float maxX = -100;
            float maxZ = -100;
            foreach (var op in objWithParent)
            {
                if (op.transform.localPosition.x > maxX) maxX = op.transform.localPosition.x;
                if (op.transform.localPosition.z > maxZ) maxZ = op.transform.localPosition.z;
            }
            Vector2Int pos = new Vector2Int(Mathf.Abs((int)maxZ), Mathf.Abs((int)maxX));
            Vector2Int posCell = pos + head; // tính toán vị trí của ô trong grid
            GridCell cell = GridManager.Instance.grid[posCell.x, posCell.y]; // lấy cell tương ứng với toạ độ
            obj.transform.position = cell.transform.position;
        }
    }
}
