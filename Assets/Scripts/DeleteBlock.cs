using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBlock : MonoBehaviour
{
    public void ExplodeBlockAndNeighBors(BlockManager currentBlock)
    {

        string currentColor = "";
        // currentColor = currentBlock.GetColorOutSite();
        List<GridCell> neighbors = currentBlock.GetNeighborOfBlock();
        foreach (GridCell neighbor in neighbors)
        {
            for (int i = 0; i < 2; i++)
            {
                Debug.Log("vòng thứ " + i);
                currentColor = currentBlock.GetColorOutSite();
                GameObject neighborTop = neighbor.PeekTopLayer();
                Debug.Log(neighbor.layers.Count);
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
                //Debug.Log(" neighbor block: " + neighborBlock.name);
                // Nếu trùng màu
                
                if (neighborBlock.GetColorOutSite() == currentColor)
                {
                    currentBlock.Explode();
                    neighborBlock.Explode();
                    Debug.Log(neighbor.layers.Count);
                    currentBlock.RemoveQuantity();
                    neighborBlock.RemoveQuantity();
                }
                
            }
        }
    }

}

