using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBlock : MonoBehaviour
{
    public void ExplodeBlockAndNeighBors(BlockManager currentBlock)
    {
        Color currentColor = currentBlock.color;
        List<GridCell> neighbors = currentBlock.GetNeighborOfBlock();

        foreach (GridCell neighbor in neighbors)
        {
            GameObject neighborTop = neighbor.PeekTopLayer();
            if (neighborTop == null) continue;

            BlockManager neighborBlock = neighborTop.GetComponentInParent<BlockManager>();
            if (neighborBlock == null) continue;

            // Nếu trùng màu
            if (neighborBlock.color == currentColor)
            {
                currentBlock.Explode();
                neighborBlock.Explode();
            }
        }
    }

}

