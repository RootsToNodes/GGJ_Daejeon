using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Balance Data", menuName = "Scriptable Object/Balance Data")]
public class SOGameBalance : ScriptableObject
{
    [SerializeField] private int _maxLevel;
    public int MaxLevel => _maxLevel;
    
    [SerializeField] private AnimationCurve _difficultCurve;
    public AnimationCurve DifficultCurve => _difficultCurve;
    
    [SerializeField] private NodeStatus _initialNodeStatus;
    public NodeStatus InitialNodeStatus => _initialNodeStatus;

    [SerializeField] private NodeStatus _maxNodeStatus;
    public NodeStatus MaxNodeStatus => _maxNodeStatus;
}
