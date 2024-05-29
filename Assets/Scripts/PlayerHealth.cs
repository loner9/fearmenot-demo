using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private KnockBack knockBack;
    private PlayerControl playerControl;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        knockBack = GetComponent<KnockBack>();
        playerControl = GetComponent<PlayerControl>();
    }

    public void takeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //player dead
            Debug.Log("is dead");
        }
    }
}
