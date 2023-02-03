using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Resourse
{
    public int money;
    public int gas;
    
    public Resourse(int money, int gas)
    {
        this.money = money;
        this.gas = gas;
    }
}
public class DataManager : Manager<DataManager>
{
    Resourse resourse;
    Resourse startResourse = new(0,0);
    bool isChecked;
    public DataManager()
    {
        GetInstance();
        resourse = startResourse;
    }
    public void GetMoney(int money) // �ڿ� ȹ��
    {
        resourse.money += money;
    }
    public void GetGas(int gas)
    {
        resourse.gas += gas;
    }
    public bool CheckResource(int money)
    {
        if (resourse.money > money)
        {
            resourse.money -= money;
            isChecked = true;
            PrintResource();
        }
        else
        {
            isChecked = false;
            SendError();
        }
        return isChecked;
    }
    public bool CheckResource(int money, int gas)
    {
        if ((resourse.money > money) && (resourse.gas > gas))
        {
            resourse.money -= money;
            resourse.gas -= gas;
            isChecked = true;
            PrintResource();

        }
        else
        {
            isChecked = false;
            SendError();
        }
        return isChecked;
    }

    private void SendError()
    {
        Debug.Log("���� ������");
    }
    private void PrintResource()
    {
        Debug.Log($"���� �ڿ��� : {resourse.money} + {resourse.gas}");
    }

}
