using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlockMaterial : MonoBehaviour
{
    public BlockMaterial blockMaterial;
    public static SpawnBlockMaterial instance;
    private void Awake()
    {
        instance = this;
    }
    public Material GetBlockMaterial(BlockMaterialType col)
    {
        foreach (var c in blockMaterial.blockMats)
        {
            if (c.blockMaterialType == col)
            {
                return c.material; 
            }
        }
        return null;
    }
}
