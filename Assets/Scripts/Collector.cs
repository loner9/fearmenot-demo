using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name != "Ground")
        {
            IItem item = collider.GetComponent<IItem>();
            if (item != null)
            {
                item.Collect();
            }
        }

    }
}
