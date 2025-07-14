using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x, y;
    public Stack<GameObject> layers = new Stack<GameObject>();

    //public void Init(int x, int y)
    //{
    //    this.x = x;
    //    this.y = y;
    //}

    public bool IsEmpty() => layers.Count == 0;

    public void AddLayer(GameObject obj)
    {
        layers.Push(obj);
    }

    public GameObject RemoveTopLayer()
    {
        return layers.Count > 0 ? layers.Pop() : null;
    }
}
