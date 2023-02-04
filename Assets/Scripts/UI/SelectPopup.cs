using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectPopup : MonoBehaviour
{
    private readonly int OpenAnimationKey = Animator.StringToHash("Open");
    private readonly int CloseAnimationKey = Animator.StringToHash("Close");

    private Node targetNode;
    private Animator animator;

    public UnityAction<Node> onClickAddChild { private get; set; }
    public UnityAction<Node> onClickHealing { private get; set; }
    public UnityAction<Node> onClickRemoveNode { private get; set; }
    public UnityAction<Node> onClickClose { private get; set; }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (targetNode != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(targetNode.transform.position);
        }
    }

    public void SetTargetNode(Node node)
    {
        animator.SetTrigger(OpenAnimationKey);
        targetNode = node;
    }

    public void OnClickClose()
    {
        animator.SetTrigger(CloseAnimationKey);
        onClickClose?.Invoke(targetNode);
    }

    public void OnClickAddChild()
    {
        onClickAddChild?.Invoke(targetNode);
        OnClickClose();
    }

    public void OnClickHealing()
    {
        onClickHealing?.Invoke(targetNode);
    }

    public void OnClickRemoveNode()
    {
        onClickRemoveNode?.Invoke(targetNode);
    }

}
