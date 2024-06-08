using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.activeSceneChanged += sceneChanged;
    }

    private void sceneChanged(Scene arg0, Scene arg1)
    {
        Debug.Log("Scene Changed : " + SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString("scene", SceneManager.GetActiveScene().name);
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        PlayerHealth health = GameObject.Find("PlayerHealth").GetComponent<PlayerHealth>();

        if (SceneManager.GetActiveScene().name.Equals("Chapter2"))
        {
            gameController.apelAmount = PlayerPrefs.GetInt("apelAmount", 0);
            gameController.drinkAmount = PlayerPrefs.GetInt("drinkAmount", 0);
            bool sun = PlayerPrefs.GetString("sun", "").Equals("True");
            bool lantern = PlayerPrefs.GetString("lantern", "").Equals("True");
            bool compass = PlayerPrefs.GetString("compass", "").Equals("True");
            gameController.isLanternCollected = lantern;
            gameController.isSunShardCollected = sun;
            Debug.Log(PlayerPrefs.GetInt("apelAmount", 0) + " " + lantern + " " + PlayerPrefs.GetString("lantern", ""));
            gameController.initialStateB();
        }
        if (SceneManager.GetActiveScene().name.Equals("BossFight"))
        {
            gameController.apelAmount = PlayerPrefs.GetInt("apelAmount", 0);
            gameController.drinkAmount = PlayerPrefs.GetInt("drinkAmount", 0);
            gameController.isLanternCollected = true;
            gameController.isSunShardCollected = true;
            gameController.isMagnetCollected = true;
            gameController.isCompassFixed = true;

        }
    }

    public void nextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // StartCoroutine(loadLevel());
    }

    public void toLevel(int level)
    {
        SceneManager.LoadSceneAsync(level);

    }

    IEnumerator loadLevel()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
