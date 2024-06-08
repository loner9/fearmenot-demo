using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour, IItem
{
    // Start is called before the first frame update
    public static event Action<bool> onMagnetCollect;
    public void Collect()
    {
        onMagnetCollect.Invoke(true);
        Destroy(gameObject);
    }
}
