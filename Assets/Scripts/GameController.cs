using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int foodAmount;
    // Start is called before the first frame update
    void Start()
    {
        foodAmount = 0;
        Food.onFoodCollect += increaseFoodAmount;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void increaseFoodAmount(int amount)
    {
        foodAmount += amount;
        Debug.Log("food : " + foodAmount);
    }
}
