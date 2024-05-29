using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //todo : improve this code to trigger animation effect on enemy
            Destroy(collision.gameObject);
        }
    }
}
