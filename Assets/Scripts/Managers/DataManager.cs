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
        Debug.Log(this.GetType().Name + ": 초기화 완료");
        resourse = startResourse;
    }
    public static void GetMoney(int money) // 자원 획득
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
        Debug.Log("조건 부적합");
    }
    private void PrintResource()
    {
        Debug.Log($"현재 자원은 : {resourse.money} + {resourse.gas}");
    }

}
