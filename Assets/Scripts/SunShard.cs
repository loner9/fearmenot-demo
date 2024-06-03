using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShard : MonoBehaviour, IItem
{
    public static event Action<bool> onSunCollect;

    public void Collect()
    {
        onSunCollect.Invoke(true);
        Destroy(gameObject);
    }
}
