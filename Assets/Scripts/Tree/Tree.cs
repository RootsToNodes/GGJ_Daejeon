using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Vector2 space;
    public Node originPrefab; //임시

    public Node roots { get; private set; }
    private readonly List<Node> leafNodeList = new List<Node>();
    
    private int treeLevel = 1;
    
    
    private void Awake()
    {
        roots = CreateNewNode(null, new NodeStatus());
        leafNodeList.Add(roots);
    }

    public List<Node> GetLeafNodes()
    {
        return leafNodeList;
    }
    
    public Node CreateNewNode(Node parent, NodeStatus status)
    {
        var node = Instantiate(originPrefab);
        node.Initialization(parent, status);

        var level = node.GetNodeLevel();
        if (level > treeLevel)
        {
            treeLevel = level;
        }

        parent?.AddChild(node);

        if (roots)
        {
            leafNodeList.Clear();
            AlignmentNodes(roots);
        }

        return node;
    }

    private void AlignmentNodes(Node curNode, int level = 1)
    {
        if (level == treeLevel)
        {
            leafNodeList.Add(curNode);
        }
        
        if (curNode.children.Count == 0)
        {
            return;
        }

        Vector2 curNodePos = curNode.transform.position;

        var length = Mathf.Pow(space.y, (treeLevel - level) + 1);
        var unit = curNode.children.Count == 1 ? 0 : length / (float) (curNode.children.Count - 1);

        for (int i = 0; i < curNode.children.Count; i++)
        {
            var child = curNode.children[i];
            var pos = new Vector2(space.x,-length * 0.5f + unit * i);

            child.transform.position = curNodePos + pos;
            child.SetLine();

            AlignmentNodes(child, level + 1);
        }
    }
}