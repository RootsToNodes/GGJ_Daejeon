using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    public static GameManager GetInstance()
    {
        return instance;
    }
    
    
    [SerializeField] private LayerMask nodeLayerMask;
    [SerializeField] private Tree tree;
    [SerializeField] EnemySpawner[] enemySpawner;

    [SerializeField] private CameraMovement cameraMove;
    [SerializeField] private MinimapCamera minimapCamera;

    [SerializeField] private SelectPopup selectPopup;

    float distanceFromNode = 20f;

    private void Awake()
    {
        instance = this;
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity,
                    nodeLayerMask))
            {
                if (hit.collider.TryGetComponent(typeof(Node), out var node))
                {
                    selectPopup.SetTargetNode((Node) node);
                    cameraMove.FocusToTarget(node.transform.position);
                }
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

        minimapCamera.UpdateMiniMapCamera(tree.treeArea);
        cameraMove.SetBorder(tree.treeArea);
    }

    private void SetSpawnerLeafNodeList()
    {
        foreach (var spawner in enemySpawner)
        {
            spawner.SetLeafNodeList(tree.GetLeafNodes());
            spawner.transform.position = new Vector2(tree.treeArea.xMax + distanceFromNode, spawner.transform.position.y);
        }
    }

    public void WaveStart()
    {
        foreach (var spawner in enemySpawner)
        {
            spawner.AttackStart();
        }
    }
}