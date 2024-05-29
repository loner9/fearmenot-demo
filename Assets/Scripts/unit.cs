using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour
{
    public string unitName;
    public int damage;
    public int maxHP;
    public int currentHP;
    [Range(0, 1)]
    public float hitChance;
    [Range(0, 1)]
    public float missChance;
    [Range(0, 1)]
    public float evadeChance;

    public bool takeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
