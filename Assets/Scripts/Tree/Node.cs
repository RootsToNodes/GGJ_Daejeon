using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct NodeStatus
{
    public enum BulletForm
    {
        None,
        Straight,
        Radial,
    }
    
    public float attackPower;
    public float attackSpeed;

    public float shotRange;
    public float rotationSpeed;
    
    public BulletForm bulletForm;
    public int bulletCount;
    public float bulletSpeed;
    public float bulletLifeTime;
    
    public float defense;

    public static NodeStatus operator +(NodeStatus a, NodeStatus b)
    {
        var status = new NodeStatus();
        status.attackPower = a.attackPower + b.attackPower;
        status.attackSpeed = a.attackSpeed + b.attackSpeed;

        status.shotRange = a.shotRange + b.shotRange;
        status.rotationSpeed = a.rotationSpeed + b.rotationSpeed;
        
        status.bulletForm = b.bulletForm == BulletForm.None ? b.bulletForm : a.bulletForm;
        status.bulletCount = a.bulletCount + b.bulletCount;
        status.bulletSpeed = a.bulletSpeed + b.bulletSpeed;
        status.bulletLifeTime = a.bulletLifeTime + b.bulletLifeTime;
        
        status.defense = a.defense + b.defense;
        
        return status;
    }
}

public class Node : MonoBehaviour
{
    private const float nodeAnimationSpeed  = 3;

    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Turret turret;
    [SerializeField] private Barrier barrier;

    private NodeStatus status;
    public NodeStatus currentStatus { get; private set; }

    public Node parent { get; private set; }
    public List<Node> children = new List<Node>();
    public int nodeLevel { get; private set; }

    public float hp { get; private set; }

    private readonly UnityEvent<int> onHealing = new UnityEvent<int>();

    public Vector2 destPosition { get; private set; }

    private void Update()
    {
        var dir = ((Vector3) destPosition - transform.position).normalized;
        transform.position += dir * nodeAnimationSpeed * Time.deltaTime;
    
        if (parent)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, parent.transform.position);
        }
    }

    public void Initialization(Node parent, NodeStatus status)
    {
        if (parent)
        {
            transform.position = parent.transform.position;
        }

        this.parent = parent;
        this.status = status;
        currentStatus = CalculateStatus(status);

        turret.Initialization(this);
        barrier.Initialization(this);

        SetEvents();
        turret.SetEnable(true);

        CalculateNodeLevel();
    }

    private void SetEvents()
    {
        onHealing.RemoveAllListeners();

        onHealing.AddListener(turret.OnHealing);
        onHealing.AddListener(barrier.OnHealing);
    }

    private NodeStatus CalculateStatus(NodeStatus status)
    {
        status += this.status;

        return parent != null ? parent.CalculateStatus(status) : status;
    }

    private int CalculateNodeLevel()
    {
        var curNode = parent;
        nodeLevel = 1;
        while (curNode != null)
        {
            nodeLevel++;
            curNode = curNode.parent;
        }

        return nodeLevel;
    }

    public void AddChild(Node child)
    {
        children.Add(child);
        
        SetEnable(false);
    }

    public void RemoveChild(Node child)
    {
        children.Remove(child);
        
        if (children.Count == 0)
        {
            SetEnable(true);
        }
    }

    public void SetEnable(bool enable)
    {
        var color = mainSprite.color;
        color.a = enable ? 1 : 0.5f;
        mainSprite.color = color;

        turret.SetEnable(enable);
    }
    
    public void SetPosition(Vector2 pos)
    {
        destPosition = pos;
    }

    public void OnDamage(float damage)
    {
        if (barrier.hp > 0)
        {
            
        }
        else if (turret.hp > 0)
        {
            
        }
        else
        {
            
        }
    }
}