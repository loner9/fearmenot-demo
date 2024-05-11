using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IItem
{
    public void Collect()
    {
        Destroy(gameObject);
    }
}
