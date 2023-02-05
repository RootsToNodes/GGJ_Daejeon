using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Balance Data", menuName = "Scriptable Object/Balance Data")]
public class SOGameBalance : ScriptableObject
{
    [SerializeField] private int _maxLevel;
    public int MaxLevel => _maxLevel;
    
    [SerializeField] private int _maxChildCount;
    public int MaxChildCount => _maxChildCount;
    
    [SerializeField] private AnimationCurve _difficultCurve;
    public AnimationCurve DifficultCurve => _difficultCurve;
    
    [SerializeField] private NodeStatus _initialNodeStatus;
    public NodeStatus InitialNodeStatus => _initialNodeStatus;

    [SerializeField] private NodeStatus _maxNodeStatus;
    public NodeStatus MaxNodeStatus => _maxNodeStatus;

    [SerializeField] private float[] _possibility;
    public float[] Possibility => _possibility;
}
