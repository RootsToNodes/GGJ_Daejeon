using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> where T : new()
{
    protected static T instance;
    
    public static T GetInstance()
    {
        if (instance == null)
        {
            instance = new T();
        }
        return instance;
    }
}
