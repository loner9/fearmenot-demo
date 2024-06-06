using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Vector2 initialPos;
    public int apelAmount = 0;
    public int drinkAmount = 0;
    public bool isSunShardCollected = false;
    public bool isLanternCollected = false;
    public bool isCompassCollected = false;
    private int currentApel;
    private int currentDrink;
    private bool currentLantern;
    private bool currentSun;
    private bool currentCompass;
    public bool isLoadFirst = false;

    private void Awake()
    {
        // DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Food.onFoodCollect += increaseFoodAmount;
        Lantern.onLanternCollect += setTrueLantern;
        SunShard.onSunCollect += setTrueSun;
        SceneManager.sceneLoaded += sceneChanged;
    }

    private void sceneChanged(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("Scene Loaded");
    }

    private void sceneChanged(Scene arg0, Scene arg1)
    {
        Debug.Log("Scene Changed");
    }

    public void initialState()
    {
        apelAmount = 0;
        drinkAmount = 0;
        isSunShardCollected = false;
        isLanternCollected = false;
        isCompassCollected = false;
    }

    public void initialStateB()
    {
        currentApel = apelAmount;
        currentDrink = drinkAmount;
        currentLantern = isLanternCollected;
        currentSun = isSunShardCollected;
        currentCompass = isCompassCollected;
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
