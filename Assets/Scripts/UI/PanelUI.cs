using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private GameObject statusUI;
    [SerializeField] private TextMeshProUGUI statusText;
    
    public void SetStatusUII(NodeStatus status)
    {
        statusUI.SetActive(true);
        statusText.text = status.GetUniqueStatusString();
    }
    
    public void CloseStatusUI()
    {
        statusUI.SetActive(false);
    }

    public void SetCostText()
    {
        
    }
}
