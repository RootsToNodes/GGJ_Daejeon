using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[Serializable]
public struct NodeStatus
{
    public enum BulletForm
    {
        None,
        Straight,
        Radial,
    }

    public int attackPower;
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

    public string GetUniqueStatusString()
    {
        string result = "";


        if (attackPower != 0) result += $"공격력 : {attackPower}\n";
        if (defense != 0) result += $"방어력 : {defense}\n";
        if (attackSpeed != 0) result += $"공격 속도 : {attackSpeed}\n";
        if (shotRange != 0) result += $"공격 범위: {shotRange}\n";
        if (rotationSpeed != 0) result += $"회전 속도 : {rotationSpeed}\n";

        if (bulletForm != 0)
        {
            switch (bulletForm)
            {
                case NodeStatus.BulletForm.Straight:
                    result += $"발사 타입 : 연사형\n";
                    break;
                case NodeStatus.BulletForm.Radial:
                    result += $"발사 타입 : 방사형\n";
                    break;
            }
        }

        if (bulletCount != 0) result += $"총알 개수 : {bulletCount}\n";
        if (bulletSpeed != 0) result += $"총알 속도 : {bulletSpeed}\n";
        if (bulletLifeTime != 0) result += $"총알 수명 : {bulletLifeTime}\n";

        return result;
    }
}

public class Node : MonoBehaviour
{
    private const float nodeAnimationSpeed = 3;

    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Turret turret;
    [SerializeField] private Barrier barrier;

    [SerializeField] private Image hpGuage;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI statusText;

    private NodeStatus status;
    public NodeStatus currentStatus { get; private set; }

    public Node parent { get; private set; }
    public List<Node> children = new List<Node>();
    public int nodeLevel { get; private set; }

    public float hp { get; private set; }

    private readonly UnityEvent<float> onHealing = new UnityEvent<float>();
    private UnityAction<Node> onDestroy;

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

    public void Initialization(Node parent, NodeStatus status, UnityAction<Node> onDestroy)
    {
        if (parent)
        {
            transform.position = parent.transform.position;
        }

        this.parent = parent;
        this.status = status;
        currentStatus = CalculateStatus(status);

        if (parent == null)
        {
            statusText.enabled = false;
        }
        else
        {
            statusText.text = status.GetUniqueStatusString();
        }

        this.onDestroy = onDestroy;

        turret.Initialization(this, 100);
        barrier.Initialization(this, 100);

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

    public void OnDamage(float amount)
    {
        SoundManager.PlaySound(AudioEnum.Hited);
        if (barrier.hp > 0)
        {
            barrier.OnDamage(amount);
        }
        else if (turret.hp > 0)
        {
            turret.OnDamage(amount);
        }
        else
        {
            hp -= Mathf.Max(amount - currentStatus.defense, 0);

            if (hp < 0)
            {
                onDestroy?.Invoke(this);
                SoundManager.PlaySound(AudioEnum.Defeat);
            }
        }
    }

    private void SetHpUI()
    {
        var tortalMaxHp = 500;
    }
}