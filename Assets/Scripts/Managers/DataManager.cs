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
        Debug.Log(this.GetType().Name + ": �ʱ�ȭ �Ϸ�");
        resourse = startResourse;
    }
    public static void GetMoney(int money) // �ڿ� ȹ��
    {
        GetInstance().resourse.money += money;
        GetInstance().PrintResource();

    }
    public static void GetGas(int gas)
    {
        GetInstance().resourse.gas += gas;
        GetInstance().PrintResource();

    }
    public static bool CheckResource(int money)
    {
        if (GetInstance().resourse.money > money)
        {
            GetInstance().resourse.money -= money;
            GetInstance().isChecked = true;
            GetInstance().PrintResource();
        }
        else
        {
            GetInstance().isChecked = false;
            GetInstance().SendError();
        }
        return GetInstance().isChecked;
    }
    public static bool CheckResource(int money, int gas)
    {
        if ((GetInstance().resourse.money > money) && (GetInstance().resourse.gas > gas))
        {
            GetInstance().resourse.money -= money;
            GetInstance().resourse.gas -= gas;
            GetInstance().isChecked = true;
            GetInstance().PrintResource();

        }
        else
        {
            GetInstance().isChecked = false;
            GetInstance().SendError();
        }
        return GetInstance().isChecked;
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
