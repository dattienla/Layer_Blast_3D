using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockLevel
{
    public List<BlockMeshType> blockMeshType;
    public List<BlockMaterialType> blockMaterialType;
    public Vector3 rotation;
    public List<Vector2> posInGrid;
}
[System.Serializable]
public class BlockOfPlayer
{
    public List<BlockMeshType> blockMeshType;
    public List<BlockMaterialType> blockMaterialType;
    public Vector3 rotation;
    public List<Vector2> posOfCubes;
}

[CreateAssetMenu(fileName = "Level", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Block của level")]
    public List<BlockLevel> blocks;

    [Header("Block của Player")]
    [Header("Block Hàng 1")]
    public List<BlockOfPlayer> blocksOfPlayer_1;
    [Header("Block Hàng 2")]
    public List<BlockOfPlayer> blocksOfPlayer_2;
    [Header("Block Hàng 3")]
    public List<BlockOfPlayer> blocksOfPlayer_3;


}
