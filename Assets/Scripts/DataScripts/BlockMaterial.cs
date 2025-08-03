using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BlockMat
{
    public BlockMaterialType blockMaterialType;
    public Material material;
}

[CreateAssetMenu(fileName = "NewBlockMaterialData", menuName = "Data/BlockMaterial")]
public class BlockMaterial : ScriptableObject
{
    public List<BlockMat> blockMats = new List<BlockMat>();
}
