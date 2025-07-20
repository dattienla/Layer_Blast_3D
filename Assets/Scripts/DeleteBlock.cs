using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBlock : MonoBehaviour
{
    public void ExplodeBlockAndNeighBors(BlockManager currentBlock)
    {
        string currentColor = "";
        currentColor = currentBlock.GetColorOutSite();
        HashSet<BlockManager> PreBlockExplode = new HashSet<BlockManager>();
        Queue<BlockManager> queue = new Queue<BlockManager>(currentBlock.GetComponent<DraggableBlock>().BlockQ);
        queue.Dequeue();
        foreach (var neighborBlock in queue)
        {
            // Nếu trùng màu
            if (neighborBlock.GetColorOutSite() == currentColor)
            {
                PreBlockExplode.Add(neighborBlock);
                PreBlockExplode.Add(currentBlock);
            }
        }
        foreach (var pre in PreBlockExplode)
        {
            pre.Explode();
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

