using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Vector2 space;

    public Node originPrefab; //임시

    public Node roots { get; private set; }


    /* ----------------------- KMS*/
    [SerializeField]
    EnemySpawner[] enemySpawner;
    private List<Node> leafNodeList = new List<Node>();
    private void SearchLeafNode(Node node)
    {
        leafNodeList.Clear();
        for (int i = 0; i < node.children.Count; i++)
        {
            if (node.children[i] == null)
            {
                leafNodeList.Add(node);
                Debug.Log(node);
            }
            else
            {
                SearchLeafNode(node.children[i]);
            }
        }
    }
    public List<Node> GetLeafNodes()
    {
        return leafNodeList;
    }

    /* ---------- 리프노드 탐색 로직 ------------- KMS*/



    private int treeLevel = 1;

    private void Awake()
    {
        roots = CreateNewNode(null, new NodeStatus());
        leafNodeList.Add(roots);
        /* ----------------------- KMS*/
        for (int i = 0; i < enemySpawner.Length; i++)
        {
            enemySpawner[i].GetLeafNode = GetLeafNodes;
        }
        /* ---------- 스포너 델리게이트 부착 ------------- KMS*/

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
        /* ----------------------- KMS*/

        SearchLeafNode(roots); // 노드 개수가 변경되는 곳 마다 이 구문을 호출해주세요
        for (int i = 0; i < length; i++)
        {
            enemySpawner[i].FindNode();
        }
        /* ----------- 변경시마다 리프노드 탐색 및 에네미 스포너에 적용 ------------ KMS*/
    }
}