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
            if (collision.name.Contains("Dive"))
            {
                EnemyPatrolDive enemy = collision.GetComponent<EnemyPatrolDive>();
                enemy.isDead = true;
                enemy.disable();
            }

            if (collision.name.Contains("Patrol"))
            {
                EnemyPatrol enemy = collision.GetComponent<EnemyPatrol>();
                enemy.isDead = true;
                enemy.disable();
            }
            // Destroy(collision.gameObject);
        }
    }
}
