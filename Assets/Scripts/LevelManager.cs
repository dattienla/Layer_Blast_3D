using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

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
        transform.DOScale(new Vector3(1f, 0.8f, 1f), 0.1f).SetEase(Ease.OutBack);
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
            obj.transform.DOMove(cell.transform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
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
            obj.transform.DOMove(cell.transform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
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
            Vector3 cubeCenter = Vector3.zero;
            foreach (var cube in objWithParent)
                cubeCenter += cube.transform.localPosition;

            cubeCenter /= objWithParent.Count;
            // Tính offset giữa pivot và mesh center trong local space
            MeshFilter mf = obj.GetComponent<MeshFilter>();
            Vector3 meshCenterLocal = mf.sharedMesh.bounds.center;
            Vector3 offset = obj.transform.localRotation * meshCenterLocal;
            Vector3 newLocalPos = cubeCenter + offset * -1f;
            int X = Mathf.RoundToInt(newLocalPos.x);
            int Z = Mathf.RoundToInt(newLocalPos.z);
            int Y = Mathf.RoundToInt(newLocalPos.y);
            obj.transform.localPosition = new Vector3(X, Y, Z);
            Vector2Int pos = new Vector2Int(Mathf.Abs(Z), Mathf.Abs(X));
            Vector2Int posCell = pos + head; // tính toán vị trí của ô trong grid
            GridCell cell = GridManager.Instance.grid[posCell.x, posCell.y]; // lấy cell tương ứng với toạ độ
            obj.transform.DOMove(cell.transform.position, 0.5f).SetEase(Ease.InBack);

        }
    }
}
