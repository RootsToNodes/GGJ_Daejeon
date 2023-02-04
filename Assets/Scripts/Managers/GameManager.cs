using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LayerMask nodeLayerMask;
    [SerializeField] private Tree tree;
    [SerializeField] EnemySpawner[] enemySpawner;

    [SerializeField] private MinimapCamera minimapCamera;
    
    [SerializeField] private SelectPopup selectPopup;

    private void Awake()
    {
        selectPopup.onClickAddChild = OnClickAddChild;
    }

    private void Start()
    {
        SetSpawnerLeafNodeList();
    }
    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                float.PositiveInfinity, nodeLayerMask);

            if (hit.collider != null && hit.collider.TryGetComponent(typeof(Node), out var node))
            {
                selectPopup.SetTargetNode((Node) node);
            }
            else
            {
                selectPopup.OnClickClose();
            }
        }
    }

    private void OnClickAddChild(Node node)
    {
        tree.CreateNewNode(node, new NodeStatus());
        SetSpawnerLeafNodeList();
        
        minimapCamera.UpdateMiniMapCamera(tree);
    }
    
    private void SetSpawnerLeafNodeList()
    {
        foreach (var spawner in enemySpawner)
        {
            spawner.SetLeafNodeList(tree.GetLeafNodes());
        }
    }
    
}