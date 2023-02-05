using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Balance Data", menuName = "Scriptable Object/Balance Data")]
public class SOGameBalance : ScriptableObject
{
    [SerializeField] private int _addChildCost;
    public int AddChildCost => _addChildCost;

    [SerializeField] private int _removeNodeCost;
    public int RemoveNodeCost => _removeNodeCost;

    [SerializeField] private int _repairCost;
    public int RepairCost => _repairCost;
    
    
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

    public NodeStatus GetRandomStatus(int level)
    {
        var status = new NodeStatus();
        
        var possibility = _difficultCurve.Evaluate(level / (float) _maxLevel);

        if (Random.value < possibility) status.attackPower = Random.Range(0, MaxNodeStatus.attackPower);
        if (Random.value < possibility) status.attackSpeed = Random.Range(0, MaxNodeStatus.attackSpeed);
        if (Random.value < possibility) status.shotRange = Random.Range(0, MaxNodeStatus.shotRange);
        if (Random.value < possibility) status.rotationSpeed = Random.Range(0, MaxNodeStatus.rotationSpeed);
        if (Random.value < possibility)
            status.bulletForm = (NodeStatus.BulletForm) Random.Range(0, (int) NodeStatus.BulletForm.Radial + 1);
        if (Random.value < possibility) status.bulletCount = Random.Range(0, MaxNodeStatus.bulletCount);
        if (Random.value < possibility) status.bulletSpeed = Random.Range(0, MaxNodeStatus.bulletSpeed);
        if (Random.value < possibility) status.bulletLifeTime = Random.Range(0, MaxNodeStatus.bulletLifeTime);
        if (Random.value < possibility) status.defense = Random.Range(0, MaxNodeStatus.defense);

        return status;
    }
}
