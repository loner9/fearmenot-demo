using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour, IItem
{
    public static event Action<bool> onCompassCollect;
    public void Collect()
    {
        onCompassCollect.Invoke(true);
        Destroy(gameObject);
    }
}
