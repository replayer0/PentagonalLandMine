using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteText : MonoBehaviour
{
    public int SortingOrder = 1;

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        renderer.sortingOrder = SortingOrder;
    }
}
