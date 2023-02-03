using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class Node : MonoBehaviour
{  
    [SerializeField] private Turret turret;
    [SerializeField] private Barrier barrier;

    public Node parent { get; private set; }
    public float hp { get; private set; }
    
    private readonly UnityEvent<int> onHealing = new UnityEvent<int>();

    public void Initialization(Node parent)
    {
        this.parent = parent;

        hp = 10;

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
}
