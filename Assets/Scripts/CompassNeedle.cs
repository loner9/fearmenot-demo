using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassNeedle : MonoBehaviour, IItem
{
    // Start is called before the first frame update
    public static event Action<bool> onCompassNeedleCollect;
    public void Collect()
    {
        onCompassNeedleCollect.Invoke(true);
        Destroy(gameObject);
    }
}
