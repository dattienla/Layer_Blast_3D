using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x, y;
    public Queue<GameObject> layers = new Queue<GameObject>();
    private Renderer rend;
    private Color defaultColor;
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;
    public bool IsEmpty() => layers.Count == 0;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        defaultColor = rend.material.color;
    }
    public void SetHighlight(bool highlight, bool isValid = true)
    {
        if (!highlight)
        {
            rend.material.color = defaultColor;
        }
        else
        {
            rend.material.color = isValid ? validColor : invalidColor;
        }
    }
    public GameObject PeekTopLayer()
    {
        return layers.Count > 0 ? layers.Peek() : null;
    }

}
