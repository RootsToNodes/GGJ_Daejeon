using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Node originPrefab; //임시
    
    public Node roots { get; private set; }
    
    
    private void Start()
    {
        CreateNewNode(null);
    }

    public void CreateNewNode(Node parent)
    {
        roots = Instantiate(originPrefab);
        roots.Initialization(parent, null);
    }
}
