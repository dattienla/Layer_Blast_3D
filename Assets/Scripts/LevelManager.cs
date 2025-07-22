using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject level1;
    Vector2Int head = new Vector2Int(1, 1); // vector của GridManager
    List<Vector2Int> gridPosOfChild = new List<Vector2Int>(); // danh sách vector tương đối của các cube con
    List<Vector2Int> posCells = new List<Vector2Int>(); // danh sách các ô mục tiêu
    GameObject[] gameObjects;
    List<GameObject> allBlockPref = new List<GameObject>();
    List<GameObject> allBlockOutPref = new List<GameObject>();
    List<GameObject> allBlockInPref = new List<GameObject>();
    private void Start()
    {
        transform.position = new Vector3(-2.5f, 0f, -15f);
        gameObjects = GameObject.FindGameObjectsWithTag("blockPref");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            allBlockPref.Add(gameObjects[i]);
        }
        // Level1();
        transform.DOScale(new Vector3(1f, 0.2f, 1f), 0.2f).SetEase(Ease.OutBack);
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
    }
    void Level1()
    {
        List<GameObject> cubeTarget = new List<GameObject>();
        List<GridCell> targetCells = new List<GridCell>();
        for (int i = 0; i < allBlockPref.Count; i++) // lấy toạ độ tương đối từng cube con và toạ độ cell tương ứng
        {
            transform.DOScale(new Vector3(1f, 0.2f, 1f), 0.2f).SetEase(Ease.OutBack);
            cubeTarget.Add(allBlockPref[i].gameObject);
            Vector2Int pos = new Vector2Int(Mathf.Abs((int)allBlockPref[i].transform.localPosition.z), Mathf.Abs((int)allBlockPref[i].transform.localPosition.x));
            Vector2Int posCell = pos + head; // tính toán vị trí của ô trong grid
            GridCell cell = GridManager.Instance.grid[posCell.x, posCell.y]; // lấy cell tương ứng với toạ độ
            targetCells.Add(cell);
            gridPosOfChild.Add(pos);
            posCells.Add(posCell);
        }
        for (int i = posCells.Count - 1; i >= 0; i--)
        {
            GridCell cell = targetCells[i];
            GameObject cube = cubeTarget[i];
            // Debug.Log("Cube: " + cube.name );
            cell.layers.Enqueue(cube); // Thêm cube vào cell
            cube.transform.DOMove(cell.transform.position, 1f).SetEase(Ease.InBack).OnComplete(() =>
            {
                cube.transform.position = cell.transform.position;
            });
            BlockManager cube_BlockManger = cubeTarget[i].transform.parent.GetComponent<BlockManager>();
            cube_BlockManger.isUsed = true; // Đánh dấu block đã được sử dụng
            // Debug.Log("Cube " + cube.name + " snapped to cell at position: " + cell.transform.position);
        }
    }
}
