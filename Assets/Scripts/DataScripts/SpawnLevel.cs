using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    public LevelData levelData;
    public GameObject cubePrefab;
    // Start is called before the first frame update
    void Start()
    {
        Push();
        BlockOfPlayer();
    }
    void Push()
    {
        GameObject obj = new GameObject("MyLevel");
        obj.AddComponent<LevelManager>();
        for (int i = 0; i < levelData.blocks.Count; i++)
        {
            GameObject block = new GameObject("Block" + i);
            block.transform.parent = obj.transform;
            block.AddComponent<BlockManager>();
            int indexBlockMeshType = 1;
            foreach (var blockMeshType in levelData.blocks[i].blockMeshType)
            {
                GameObject blockMesh = Instantiate(SpawnBlockMesh.instance.GetBlockMesh(blockMeshType),
                    block.transform.position, Quaternion.Euler(levelData.blocks[i].rotation), block.transform);
                if (levelData.blocks[i].blockMeshType.Count == 1)
                {
                    blockMesh.tag = "cubeMatOut";
                    blockMesh.GetComponent<Renderer>().material
                        = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocks[i].blockMaterialType[0]);
                    foreach (var c in levelData.blocks[i].posInGrid)
                    {
                        GameObject cube = Instantiate(cubePrefab,
                             block.transform.position, Quaternion.identity, block.transform);
                        cube.GetComponent<Renderer>().material
                            = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocks[i].blockMaterialType[0]);
                        Vector3 pos = cube.transform.localPosition;
                        pos.x = c.y - 1;
                        pos.z = -(c.x - 1);
                        cube.transform.localPosition = pos;
                        cube.tag = "cubeOut";
                    }
                }
                else
                {
                    if (indexBlockMeshType == 1)
                    {
                        blockMesh.tag = "cubeMatIn";
                        blockMesh.GetComponent<Renderer>().material
                            = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocks[i].blockMaterialType[0]);
                        foreach (var c in levelData.blocks[i].posInGrid)
                        {
                            GameObject cube = Instantiate(cubePrefab,
                                 block.transform.position, Quaternion.identity, block.transform);
                            cube.GetComponent<Renderer>().material
                                = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocks[i].blockMaterialType[0]);
                            Vector3 pos = cube.transform.localPosition;
                            pos.x = c.y - 1;
                            pos.z = -(c.x - 1);
                            cube.transform.localPosition = pos;
                            cube.tag = "cubeIn";
                        }
                    }
                    else if (indexBlockMeshType == 2)
                    {
                        blockMesh.tag = "cubeMatOut";
                        Material[] mats = blockMesh.GetComponent<Renderer>().materials;
                        if (mats.Length >= 2)
                        {
                            mats[0] = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocks[i].blockMaterialType[0]); ; // gán material vào Element 0
                            mats[1] = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocks[i].blockMaterialType[1]); ; // gán material vào Element 1
                            blockMesh.GetComponent<Renderer>().materials = mats;
                        }
                        foreach (var c in levelData.blocks[i].posInGrid)
                        {
                            GameObject cube = Instantiate(cubePrefab,
                                 block.transform.position, Quaternion.identity, block.transform);
                            cube.GetComponent<Renderer>().material
                                = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocks[i].blockMaterialType[1]);
                            Vector3 pos = cube.transform.localPosition;
                            pos.x = c.y - 1;
                            pos.z = -(c.x - 1);
                            cube.transform.localPosition = pos;
                            cube.tag = "cubeOut";
                        }
                    }
                    indexBlockMeshType++;
                }
            }
        }
    }
    void BlockOfPlayer()
    {
        GameObject parent = new GameObject("BlockQueue_1");
        QueueBlockManager.Instance.blockOfPlayer[0] = parent;
        QueueBlockManager.Instance.blockOfPlayer[1] = parent;
        QueueBlockManager.Instance.blockOfPlayer[2] = parent;
        for (int i = 0; i < levelData.blocksOfPlayer_1.Count; i++)
        {
            GameObject block = new GameObject("Block" + i);
            block.AddComponent<BoxCollider>();
            block.transform.parent = parent.transform;
            block.AddComponent<BlockManager>();
            block.AddComponent<DraggableBlock>();
            block.AddComponent<DoTweenAnim>();
            block.tag = "block";
            block.GetComponent<DoTweenAnim>().index = 1;
            int indexBlockMeshType = 1;
            foreach (var blockMeshType in levelData.blocksOfPlayer_1[i].blockMeshType)
            {
                GameObject blockMesh = Instantiate(SpawnBlockMesh.instance.GetBlockMesh(blockMeshType),
                    block.transform.position, Quaternion.Euler(levelData.blocksOfPlayer_1[i].rotation), block.transform);
                if (levelData.blocksOfPlayer_1[i].blockMeshType.Count == 1)
                {
                    blockMesh.tag = "cubeMatOut";
                    blockMesh.GetComponent<Renderer>().material
                        = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocksOfPlayer_1[i].blockMaterialType[0]);
                    foreach (var c in levelData.blocksOfPlayer_1[i].posOfCubes)
                    {
                        GameObject cube = Instantiate(cubePrefab,
                             block.transform.position, Quaternion.identity, block.transform);
                        cube.GetComponent<Renderer>().material
                            = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocksOfPlayer_1[i].blockMaterialType[0]);
                        Vector3 pos = cube.transform.localPosition;
                        pos.x = c.x;
                        pos.z = c.y;
                        cube.transform.localPosition = pos;
                        cube.tag = "cubeOut";
                    }
                }
                else
                {
                    if (indexBlockMeshType == 1)
                    {
                        blockMesh.tag = "cubeMatIn";
                        blockMesh.GetComponent<Renderer>().material
                            = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocksOfPlayer_1[i].blockMaterialType[0]);
                        foreach (var c in levelData.blocksOfPlayer_1[i].posOfCubes)
                        {
                            GameObject cube = Instantiate(cubePrefab,
                                 block.transform.position, Quaternion.identity, block.transform);
                            cube.GetComponent<Renderer>().material
                                = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocksOfPlayer_1[i].blockMaterialType[0]);
                            Vector3 pos = cube.transform.localPosition;
                            pos.x = c.x;
                            pos.z = c.y;
                            cube.transform.localPosition = pos;
                            cube.tag = "cubeIn";
                        }
                    }
                    else if (indexBlockMeshType == 2)
                    {
                        blockMesh.tag = "cubeMatOut";
                        Material[] mats = blockMesh.GetComponent<Renderer>().materials;
                        if (mats.Length >= 2)
                        {
                            mats[0] = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocksOfPlayer_1[i].blockMaterialType[0]); ; // gán material vào Element 0
                            mats[1] = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocksOfPlayer_1[i].blockMaterialType[1]); ; // gán material vào Element 1
                            blockMesh.GetComponent<Renderer>().materials = mats;
                        }
                        foreach (var c in levelData.blocksOfPlayer_1[i].posOfCubes)
                        {
                            GameObject cube = Instantiate(cubePrefab,
                                 block.transform.position, Quaternion.identity, block.transform);
                            cube.GetComponent<Renderer>().material
                                = SpawnBlockMaterial.instance.GetBlockMaterial(levelData.blocksOfPlayer_1[i].blockMaterialType[1]);
                            Vector3 pos = cube.transform.localPosition;
                            pos.x = c.x;
                            pos.z = c.y;
                            cube.transform.localPosition = pos;
                            cube.tag = "cubeOut";
                        }
                    }
                    indexBlockMeshType++;
                }
            }
        }
    }
}
