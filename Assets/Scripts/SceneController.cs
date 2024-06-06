using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnim;

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
        if (SceneManager.GetActiveScene().name.Equals("Chapter2"))
        {
            GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
            gameController.apelAmount = PlayerPrefs.GetInt("apelAmount", 0);
            gameController.drinkAmount = PlayerPrefs.GetInt("drinkAmount", 0);
            bool sun = PlayerPrefs.GetString("sun", "").Equals("True");
            bool lantern = PlayerPrefs.GetString("lantern", "").Equals("True");
            bool compass = PlayerPrefs.GetString("compass", "").Equals("True");
            gameController.isLanternCollected = lantern;
            gameController.isSunShardCollected = sun;
            Debug.Log(PlayerPrefs.GetInt("apelAmount", 0) + " " + lantern + " " + PlayerPrefs.GetString("lantern", ""));
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
        // transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        // transitionAnim.SetTrigger("start");
    }


}
