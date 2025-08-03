using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockMes
{
    public BlockMeshType blockMeshType;
    public GameObject blockMesh;

}

[CreateAssetMenu(fileName = "NewBlockMeshData", menuName = "Data/BlockMesh")]
public class BlockMesh : ScriptableObject
{
    public List<BlockMes> blockMeshs = new List<BlockMes>();
}
