using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeleteBlock : MonoBehaviour
{
    public HashSet<BlockManager> PreBlockExplode;
    public bool isExplode = true;
    public int cnt = 0;
    public bool isDo = false;
    private void Start()
    {
        PreBlockExplode = new HashSet<BlockManager>();
    }
    public void ExplodeBlockAndNeighBors(BlockManager currentBlock)
    {
        isExplode = false;
        Color currentColor = currentBlock.GetColorOutSite();
        Debug.Log(GetBlockNeighbor(currentBlock).Count);
        foreach (var neighborBlock in GetBlockNeighbor(currentBlock))
        {
            Debug.Log("Neighbor " + neighborBlock.name);
            // Nếu trùng màu
            if (neighborBlock.GetColorOutSite() == currentColor)
            {
                currentBlock.RemoveQuantity(); ;
                neighborBlock.RemoveQuantity();
                PreBlockExplode.Add(neighborBlock);
                PreBlockExplode.Add(currentBlock);
                cnt++;
            }
        }
        if (cnt == 0)
        {
            isExplode = false;
        }
        else
        {
            isExplode = true;
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

