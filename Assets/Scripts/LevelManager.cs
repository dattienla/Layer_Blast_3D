using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LevelManager : MonoBehaviour
{
    public GameObject obj;
    [SerializeField]
    private GameObject level1;
    Vector2Int head = new Vector2Int(1, 1); // vector của GridManager
    List<Vector2Int> gridPosOfChild = new List<Vector2Int>(); // danh sách vector tương đối của các cube con
    List<Vector2Int> posCells = new List<Vector2Int>(); // danh sách các ô mục tiêu
    GameObject[] allBlockPref;
    private void Start()
    {
        transform.position = new Vector3(-2.5f, 0f, -15f);
        allBlockPref = GameObject.FindGameObjectsWithTag("blockPref");
        Level1();
    }
    private void Update()
    {
        Debug.Log(obj.GetComponent<GridCell>().layers.Count);
    }
    void Level1()
    {
        List<GameObject> cubeTarget = new List<GameObject>();
        List<GridCell> targetCells = new List<GridCell>();
        for (int i = 0; i < allBlockPref.Length; i++) // lấy toạ độ tương đối từng cube con và toạ độ cell tương ứng
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
        for (int i = 0; i < posCells.Count; i++)
        {
            GridCell cell = targetCells[i];
            GameObject cube = cubeTarget[i];
            // Debug.Log("Cube: " + cube.name );
            cell.layers.Enqueue(cube); // Thêm cube vào cell
            GameObject neighborTop = cell.PeekTopLayer();

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
