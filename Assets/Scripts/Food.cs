using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IItem
{
    public static event Action<int, string> onFoodCollect;
    public int val = 1;
    public string type;

    public void Collect()
    {
        onFoodCollect.Invoke(val, type);
        Destroy(gameObject);
    }
}
