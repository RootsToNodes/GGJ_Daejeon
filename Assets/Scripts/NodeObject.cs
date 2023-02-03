using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeObject : MonoBehaviour
{
    public float hp { get; private set; }

    private Node node;
    
    public virtual void Initialization(Node node)
    {
        this.node = node;
    }


    public abstract void OnDamage(int amount);
    public abstract void OnHealing(int amount);
}
