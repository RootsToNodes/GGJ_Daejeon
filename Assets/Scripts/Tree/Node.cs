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
        status.attackSpeed = a.attackSpeed - b.attackSpeed;

        status.shotRange = a.shotRange + b.shotRange;
        status.rotationSpeed = a.rotationSpeed + b.rotationSpeed;

        status.bulletForm = b.bulletForm == BulletForm.None ? a.bulletForm : b.bulletForm;
        status.bulletCount = a.bulletCount + b.bulletCount;
        status.bulletSpeed = a.bulletSpeed + b.bulletSpeed;
        status.bulletLifeTime = a.bulletLifeTime + b.bulletLifeTime;

        status.defense = a.defense + b.defense;

        return status;
    }

    public string GetUniqueStatusString()
    {
        string result = "";

        if (attackPower != 0) result += string.Format("공격력 : {0:0.##}\n", attackPower);
        if (defense != 0) result += string.Format("방어력 : {0:0.##}\n", defense);
        if (attackSpeed != 0) result += string.Format("공격 속도 : {0:0.##}\n", attackSpeed);
        if (shotRange != 0) result += string.Format("공격 범위 : {0:0.##}\n", shotRange);
        if (rotationSpeed != 0) result += string.Format("회전 속도 : {0:0.##}\n", rotationSpeed);
        
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
        
        if (bulletCount != 0) result += string.Format("총알 개수 : {0:0.##}\n", bulletCount);
        if (bulletSpeed != 0) result += string.Format("총알 속도 : {0:0.##}\n", bulletSpeed);
        if (bulletLifeTime != 0) result += string.Format("총알 수명 : {0:0.##}\n", bulletLifeTime);

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

    public float hp { get; private set; } = 25;

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
        currentStatus = CalculateStatus(new NodeStatus());

        if (parent == null)
        {
            statusText.enabled = false;
        }
        else
        {
            statusText.text = status.GetUniqueStatusString();
        }

        this.onDestroy = onDestroy;

        turret.Initialization(this, 25);
        barrier.Initialization(this, 50);

        SetEvents();
        turret.SetEnable(true);

        CalculateNodeLevel();
        SetHpUI();
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

    public void Repair()
    {
        turret.OnHealing(25);
        barrier.OnHealing(50);;
        hp = 25;
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
            hp -= amount;
            //hp -= Mathf.Max(amount - currentStatus.defense, 0);

            if (hp <= 0)
            {
                onDestroy?.Invoke(this);
                SoundManager.PlaySound(AudioEnum.Defeat);
            }
        }

        SetHpUI();
    }

    private void SetHpUI()
    {
        var currentHP = hp + barrier.hp + turret.hp;
        hpGuage.fillAmount = currentHP / 100;

        hpText.text = $"{currentHP}/{100}";
    }
}