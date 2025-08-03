using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnBlockMesh : MonoBehaviour
{
    public BlockMesh blockMesh;
    public static SpawnBlockMesh instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject GetBlockMesh(BlockMeshType mesh)
    {
        foreach (var c in blockMesh.blockMeshs)
        {
            if (c.blockMeshType == mesh)
            {
                return c.blockMesh;
            }
        }
        return null;
    }
}
