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
    private Resourse resourse;
    private Resourse startResourse = new(0,0);
    
    public DataManager()
    {
        resourse = startResourse;
    }
    public static void GetMoney(int money) // �ڿ� ȹ��
    {
        GetInstance().resourse.money += money;
        SoundManager.PlaySound(AudioEnum.Money);
        GetInstance().PrintResource();

    }
    public static void GetGas(int gas)
    {
        GetInstance().resourse.gas += gas;
        GetInstance().PrintResource();

    }
    public static bool CheckResource(int money)
    {
        var instance = GetInstance();
        
        if (instance.resourse.money > money)
        {
            instance.resourse.money -= money;
            instance.PrintResource();
            return true;
        }
        else
        {
            instance.SendError();
            return false;
        }
    }
    public static bool CheckResource(int money, int gas)
    {
        var instance = GetInstance();
        
        if ((instance.resourse.money > money) && (instance.resourse.gas > gas))
        {
            instance.resourse.money -= money;
            instance.resourse.gas -= gas;
            instance.PrintResource();

            return true;
        }
        else
        {
            instance.SendError();

            return false;
        }
    }

    private void SendError()
    {
        Debug.Log("자원이 부족합니다");
    }
    private void PrintResource()
    {
        Debug.Log($"현재 자원 {resourse.money} / {resourse.gas}");
    }

}
