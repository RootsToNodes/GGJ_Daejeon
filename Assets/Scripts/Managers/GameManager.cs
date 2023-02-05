using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject overPanel;
    int wave = 0;
    public int Wave
    {
        get
        {
            for (int i = 0; i < enemySpawner.Length; i++)
            {
                if (wave == 10)
                {
                    while (enemySpawner[i]?.enemiseList.Count != 0)
                    {
                    }
                    overPanel.SetActive(true);
                }
            }
            
            return wave; } set { wave = value; } }

    
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
        selectPopup.onClickHealing = OnClickRepairButton;
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
        SoundManager.PlaySound(AudioEnum.Build);
        if (!DataManager.CheckResource(balance.AddChildCost))
        {
            return;
        }
        
        tree.CreateNewNode(node, Balance.GetRandomStatus(node.nodeLevel));
        SetSpawnerLeafNodeList();

        minimapCamera.UpdateMiniMapCamera(tree.treeArea);
        cameraMove.SetBorder(tree.treeArea);
    }

    private void OnClickRemoveNode(Node node)
    {
        if (!DataManager.CheckResource(balance.RemoveNodeCost))
        {
            return;
        }
        
        SoundManager.PlaySound(AudioEnum.Cut);
        tree.RemoveNode(node);
        SetSpawnerLeafNodeList();
    }

    private void OnClickRepairButton(Node node)
    {
        if (!DataManager.CheckResource(balance.RepairCost))
        {
            return;
        }
        
        node.Repair();;
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

        Wave++;
        
        panelUI.UpdateWaveText(Wave, 10);
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
    
    public void UpdatePanelUI()
    {
        panelUI.UpdateCostText();
    }
}