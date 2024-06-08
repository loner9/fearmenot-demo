using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour
{
    public string unitName;
    public int damage;
    private int maxHP;
    private int currentHP;
    [Range(0, 1)]
    public float hitChance;
    [Range(0, 1)]
    public float missChance;
    [Range(0, 1)]
    public float evadeChance;
    PlayerHealth health;

    private void Awake()
    {

    }

    public bool takeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        else
        {
            // currentHP = 0;
            return false;
        }
    }

    public void setMaxHealth(int health)
    {
        maxHP = health;
    }

    public int getMaxHealth()
    {
        return maxHP;
    }

    public void setHealth(int amount)
    {
        currentHP = amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public int getHealth()
    {
        return currentHP;
    }
}
