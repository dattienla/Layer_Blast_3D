using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBlock : MonoBehaviour
{
    public void ExplodeBlockAndNeighBors(BlockManager currentBlock)
    {
        string currentColor = currentBlock.color;
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
                Debug.Log("0");
                neighborBlock = neighborTop.transform.parent.GetComponent<BlockManager>();
            }
            else
            {
                Debug.Log("1");
                neighborBlock = neighborTop.GetComponent<BlockManager>();
            }
            if (neighborBlock == null) continue;
            //Debug.Log(" neighbor block: " + neighborBlock.name);
            // Nếu trùng màu
            if (neighborBlock.color == currentColor)
            {
                currentBlock.Explode();
                neighborBlock.Explode();
            }
        }
    }

}

