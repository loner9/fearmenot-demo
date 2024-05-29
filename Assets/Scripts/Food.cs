using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IItem
{
    public static event Action<int> onFoodCollect;
    public int val = 1;

    public void Collect()
    {
        onFoodCollect.Invoke(val);
        Destroy(gameObject);
    }
}
