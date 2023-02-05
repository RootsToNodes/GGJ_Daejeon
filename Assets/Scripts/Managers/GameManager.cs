using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int wave { get; private set; } = 0;
    
    public static GameManager instance { get; private set; }
    
    [SerializeField] private LayerMask nodeLayerMask;
    [SerializeField] private Tree tree;
    [SerializeField] EnemySpawner[] enemySpawner;

    [SerializeField] private CameraMovement cameraMove;
    [SerializeField] private MinimapCamera minimapCamera;

    [SerializeField] private SelectPopup selectPopup;
    [SerializeField] private PanelUI panelUI;

    [SerializeField] private SOGameBalance balance;
    public SOGameBalance Balance => balance;
    
    float distanceFromNode = 20f;

    private void Awake()
    {
        instance = this;
        selectPopup.onClickAddChild = OnClickAddChild;
        selectPopup.onClickRemoveNode = OnClickRemoveNode;
        selectPopup.onClickClose = OnClickCloseButton;
        //selectPopup.onClickHealing = 
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
                    selectPopup.SetTargetNode((Node) node,((Node) node).children.Count < balance.MaxChildCount,node != tree.roots);
                    panelUI.SetStatusUII(((Node)node).currentStatus);
                    
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
        tree.CreateNewNode(node, Balance.GetRandomStatus(node.nodeLevel));
        SetSpawnerLeafNodeList();

        minimapCamera.UpdateMiniMapCamera(tree.treeArea);
        cameraMove.SetBorder(tree.treeArea);
    }

    private void OnClickRemoveNode(Node node)
    {
        tree.RemoveNode(node);
        SetSpawnerLeafNodeList();
    }

    private void OnClickCloseButton(Node node)
    {
        panelUI.CloseStatusUI();
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

        wave++;
    }

    public Enemy GetEnemyInRange(Vector2 position, float range)
    {
        foreach (var spawner in enemySpawner)
        {
            foreach (var enemy in spawner.enemiseList)
            {
                if ((position - (Vector2)enemy.transform.position).sqrMagnitude < range * range)
                {
                    return enemy;
                }
            }
        }

        return null;
    }
}