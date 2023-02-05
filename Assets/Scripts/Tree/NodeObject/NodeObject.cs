using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class NodeObject : MonoBehaviour
{
    public float hp { get; protected set; }

    protected Node node;
    
    public virtual void Initialization(Node node, float hp)
    {
        this.hp = hp;
        this.node = node;
    }

    public abstract void OnDamage(float amount);
    public abstract void OnHealing(float amount);
}
