using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.GetInstance().GetMoney(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
