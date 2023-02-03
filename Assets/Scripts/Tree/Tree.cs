using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Vector2 space;

    public Node originPrefab; //임시

    public Node roots { get; private set; }

    private int treeLevel = 1;

    private void Start()
    {
        roots = CreateNewNode(null, new NodeStatus());
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
            AlignmentNodes(roots);
        }

        return node;
    }

    public void AlignmentNodes(Node curNode, int level = 1)
    {
        if (curNode.children.Count == 0)
        {
            return;
        }

        Vector2 curNodePos = curNode.transform.position;

        //var length = space.x * (treeLevel - level);
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