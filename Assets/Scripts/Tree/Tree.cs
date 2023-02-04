using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Rect treeArea { get; private set; }

    public Vector2 space;
    public Node originPrefab; //임시

    public Node roots { get; private set; }
    private readonly List<Node> leafNodeList = new List<Node>();

    private int treeLevel = 1;


    private void Awake()
    {
        var status = new NodeStatus
        {
            attackPower = 1,
            attackSpeed = 1,
            shotRange = 5,
            rotationSpeed = 10,
            bulletSpeed = 1,
            bulletCount = 1,
            bulletLifeTime = 5,
            defense = 5
        };

        roots = CreateNewNode(null, status);
        leafNodeList.Add(roots);
    }

    public List<Node> GetLeafNodes()
    {
        return leafNodeList;
    }

    public Node CreateNewNode(Node parent, NodeStatus status)
    {
        var node = Instantiate(originPrefab, transform);
        node.Initialization(parent, status, RemoveNode);

        if (node.nodeLevel > treeLevel)
        {
            treeLevel = node.nodeLevel;
        }

        parent?.AddChild(node);

        if (roots)
        {
            AlignmentNodes();
        }

        return node;
    }

    public void RemoveNode(Node target)
    {
        if (target == roots)
        {
            return;
        }

        List<Node> nodes = new List<Node>();

        void GetAllChildren(Node curNode)
        {
            nodes.Add(curNode);

            foreach (var node in curNode.children)
            {
                GetAllChildren(node);
            }
        }

        target.parent.RemoveChild(target);

        GetAllChildren(target);

        foreach (var node in nodes)
        {
            Destroy(node.gameObject);
        }

        AlignmentNodes();
    }

    private void AlignmentNodes()
    {
        leafNodeList.Clear();
        var area = new Rect();
        AlignmentNodes(roots, ref area);
        treeArea = area;
    }

    private void AlignmentNodes(Node curNode, ref Rect area, int level = 1)
    {
        Vector2 curNodePos = curNode.destPosition;

        if (curNodePos.x < area.xMin) area.xMin = curNodePos.x;
        if (curNodePos.x > area.xMax) area.xMax = curNodePos.x;
        if (curNodePos.y < area.yMin) area.yMin = curNodePos.y;
        if (curNodePos.y > area.yMax) area.yMax = curNodePos.y;

        if (curNode.children.Count == 0)
        {
            leafNodeList.Add(curNode);
            return;
        }

        var length = Mathf.Abs(Mathf.Pow(space.y, (treeLevel - level) + 1));
        var unit = curNode.children.Count == 1 ? 0 : length / (float) (curNode.children.Count - 1);

        for (int i = 0; i < curNode.children.Count; i++)
        {
            var child = curNode.children[i];
            var pos = new Vector2(space.x, -length * 0.5f + unit * i);

            child.SetPosition(curNodePos + pos);

            AlignmentNodes(child, ref area, level + 1);
        }
    }
}