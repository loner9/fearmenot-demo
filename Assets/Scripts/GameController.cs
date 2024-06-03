using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int apelAmount;
    int drinkAmount;
    bool isSunShardCollected = false;
    public bool isLanternCollected = false;
    bool isCompassCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        apelAmount = 0;
        drinkAmount = 0;
        Food.onFoodCollect += increaseFoodAmount;
        Lantern.onLanternCollect += setTrueLantern;
        SunShard.onSunCollect += setTrueSun;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setTrueLantern(bool value)
    {
        isLanternCollected = value;
    }

    void setTrueSun(bool value)
    {
        isSunShardCollected = value;
    }
    void increaseFoodAmount(int amount, string type)
    {
        if (type.Equals("apel"))
        {
            apelAmount += amount;

        }
        else if (type.Equals("drink"))
        {
            drinkAmount += amount;
        }
    }
}
