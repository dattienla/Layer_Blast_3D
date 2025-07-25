using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    private GameObject[] blockOfPlayer;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        allBlocks = GameObject.FindGameObjectsWithTag(blockTag);
    }

    private void Update()
    {
        CheckAndAddBlock();
        //CheckEndGame();
    }
    // kiểm tra và thêm block vào hàng đợi
    void CheckAndAddBlock()
    {
        if (queueBlock.Count == 0)
        {
            PushBlock();
        }
    }
    // thêm 3 block vào hàng đợi
    private void AddBlockToQueue()
    {
        for (int i = 0; i < blockOfPlayer.Length; i++)
        {
            Transform parent = blockOfPlayer[i].transform;

            if (parent.childCount == 0) continue; // bỏ qua nếu không có con

            Transform child = parent.GetChild(0);

            if (!queueBlock.Contains(child.gameObject)) // tránh thêm trùng
            {
                queueBlock.Add(child.gameObject);
            }
            child.SetParent(null); // tách ra khỏi cha
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
        for (int i = 0; i < blockOfPlayer.Length; i++)
        {
            Transform parent = blockOfPlayer[i].transform;

            if (parent.childCount == 0) continue; // bỏ qua nếu không có con

            Transform child = parent.GetChild(0);

            BlockManager blockManager = child.GetComponent<BlockManager>();
            DoTweenAnim doTweenAnim = child.GetComponent<DoTweenAnim>();

            if (!blockManager.isUsed && doTweenAnim.index == i + 1)
            {
                doTweenAnim.BlockStart();
            }
        }
        AddBlockToQueue();

    }
    // check end game
    public void CheckEndGame()
    {
        int n = 0;
        for (int i = 0; i < 3; i++)
        {
            if (blockOfPlayer[i].transform.childCount == 0)
            {
                n++;
            }
        }
        if (n == 3 && queueBlock.Count == 0)
        {
            GameManager.Instance.LoseGamePanel();
            Debug.Log("End Game");
        }
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
                    for (int i = 0; i < blockManager.GetCubeOutSite().Count; i++)
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
