using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBlock : MonoBehaviour
{
    public HashSet<BlockManager> PreBlockExplode;
    public static bool isExplode = true;
    private void Start()
    {
        PreBlockExplode = new HashSet<BlockManager>();
    }
    public void ExplodeBlockAndNeighBors(BlockManager currentBlock)
    {
        isExplode = false;
        string currentColor = "";
        currentColor = currentBlock.GetColorOutSite();
        foreach (var neighborBlock in GetBlockNeighbor(currentBlock))
        {
            // Nếu trùng màu
            if (neighborBlock.GetColorOutSite() == currentColor)
            {
                PreBlockExplode.Add(neighborBlock);
                PreBlockExplode.Add(currentBlock);
                isExplode = true;
            }
        }
    }

    // Lấy ra tất cả các block hàng xóm cấp 1 của 1 block truyền vào
    public HashSet<BlockManager> GetBlockNeighbor(BlockManager currentBlock)
    {
        HashSet<BlockManager> H = new HashSet<BlockManager>();
        List<GridCell> neighbors = currentBlock.GetNeighborOfBlock();
        foreach (GridCell neighbor in neighbors)
        {
            GameObject neighborTop = neighbor.PeekTopLayer();
            if (neighborTop == null || neighborTop.transform.parent == null) continue;
            //GameObject neighborParent = neighborTop.transform.parent.gameObject;
            //if (neighborParent == null) continue;

            BlockManager neighborBlock = null;

            if (neighborTop.transform.parent != null && neighborTop.transform.parent.tag != "level")
            {
                neighborBlock = neighborTop.transform.parent.GetComponent<BlockManager>();
            }
            else
            {
                neighborBlock = neighborTop.GetComponent<BlockManager>();
            }
            if (neighborBlock == null) continue;
            H.Add(neighborBlock);
        }
        return H;
    }
}

