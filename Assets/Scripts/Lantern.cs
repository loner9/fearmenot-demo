using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour, IItem
{
    public static event Action<bool> onLanternCollect;
    public void Collect()
    {
        onLanternCollect.Invoke(true);
        Destroy(gameObject);
    }
}
