using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct NodeStatus
{
    public int attackPower;
    public int attackSpeed;
    public int defense;

    public static NodeStatus operator +(NodeStatus a, NodeStatus b)
    {
        var status = new NodeStatus();
        status.attackPower = a.attackPower + b.attackPower;
        status.attackSpeed = a.attackSpeed + b.attackSpeed;
        status.defense = a.defense + b.defense;

        return status;
    }
}

public class Node : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Turret turret;
    [SerializeField] private Barrier barrier;

    private NodeStatus status;
    public NodeStatus currentStatus { get; private set; }

    public Node parent { get; private set; }
    public List<Node> children = new List<Node>();

    public float hp { get; set; }


    private readonly UnityEvent<int> onHealing = new UnityEvent<int>();

    public void Initialization(Node parent, NodeStatus status)
    {
        this.parent = parent;
        this.status = status;
        currentStatus = CalculateStatus(status);

        turret.Initialization(this);
        barrier.Initialization(this);

        SetEvents();
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

    public int GetNodeLevel()
    {
        var curNode = parent;
        var level = 1;
        while (curNode != null)
        {
            level++;
            curNode = curNode.parent;
        }

        return level;
    }

    public void AddChild(Node child)
    {
        children.Add(child);
    }

    public void RemoveChild(Node child)
    {
        children.Remove(child);
    }

    public void SetLine()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, parent.transform.position);
    }
}