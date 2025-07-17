using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBlockManager : MonoBehaviour
{
    public static QueueBlockManager Instance;
    public List<GameObject> queueBlock = new List<GameObject>();
    private string blockTag = "block";
    GameObject[] allBlocks;
    private Vector3 slot1 = new Vector3(-2.5f, 0, -4);
    private Vector3 slot2 = new Vector3(0, 0, -4);
    private Vector3 slot3 = new Vector3(2, 0, -4);
    [SerializeField]
    private GameObject[] block_;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        allBlocks = GameObject.FindGameObjectsWithTag(blockTag);
        //PushBlock();
    }
    private void Update()
    {
        // DeleteBlockFromQueue();
        if (queueBlock.Count == 0)
        {
            PushBlock();
        }
        //CheckEndGame();
    }
    // thêm 3 block vào hàng đợi
    private void AddBlockToQueue()
    {
        float threshold = 0.8f; // khoảng cách cho phép

        for (int i = 0; i < allBlocks.Length; i++)
        {
            Vector3 pos = allBlocks[i].transform.position;

            if (Vector3.Distance(pos, slot1) < threshold ||
                Vector3.Distance(pos, slot2) < threshold ||
                Vector3.Distance(pos, slot3) < threshold)
            {
                if (!queueBlock.Contains(allBlocks[i])) // tránh thêm trùng
                {
                    queueBlock.Add(allBlocks[i]);
                }
            }
            if (queueBlock.Count == 3)
            {
                return;
            }
        }
    }
    // xóa block đã sử dụng khỏi hàng đợi
    public void DeleteBlockFromQueue()
    {
        for (int i = queueBlock.Count - 1; i >= 0; i--)
        {
            BlockManager blockManager = queueBlock[i].GetComponent<BlockManager>();
            if (blockManager.isUsed)
            {
                queueBlock.RemoveAt(i);
            }
        }
    }

    // kiểm soát số lượng block hiển thị trên màn hình
    void PushBlock()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (var block in allBlocks)
            {
                BlockManager blockManger = block.GetComponent<BlockManager>();
                DoTweenAnim doTweenAnim = block.GetComponent<DoTweenAnim>();
                if (blockManger.isUsed == false && doTweenAnim.index == i + 1)
                {
                    doTweenAnim.BlockStart();
                    break;
                }
            }
        }
        AddBlockToQueue();

    }
    // check end game
    public void CheckEndGame()
    {
        int cnt = queueBlock.Count;
        foreach (GameObject block in queueBlock)
        {
            int count = 0;
            BlockManager blockManager = block.GetComponent<BlockManager>();
            for (int x = 1; x <= GridManager.Instance.width; x++)
            {
                for (int y = 1; y <= GridManager.Instance.height; y++)
                {
                    bool isValid = true;
                    Vector2Int head = new Vector2Int(x, y);
                    for (int i = 0; i < blockManager.GetCubes().Count; i++)
                    {
                        Vector2Int pos = head + blockManager.GetPositionOfCubes()[i];
                        // Debug.Log("Checking position: " + blockManager.GetPositionOfCubes()[i]);
                        if (GridManager.Instance.grid[pos.x, pos.y] == null || GridManager.Instance.grid[pos.x, pos.y].IsEmpty() == false)
                        {
                            isValid = false;
                            break;
                        }
                    }
                    if (isValid)
                    {
                        count++;
                    }
                }
            }
            if (count == 0)
            {
                cnt--;
            }
        }
        if (queueBlock.Count != 0 && cnt == 0)
        {
            GameManager.Instance.LoseGamePanel();
            Debug.Log("End Game");
        }
    }
}
