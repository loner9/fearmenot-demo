using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        // SceneManager.LoadSceneAsync(1);
        SceneController.instance.nextLevel();
    }

    public void Continue()
    {
        string scene = PlayerPrefs.GetString("scene", "");
        string save = PlayerPrefs.GetString("save", "");
        if (save.Equals("yes"))
        {
            switch (scene)
            {
                case "Chapter1":
                    SceneController.instance.toLevel(3);
                    break;
                case "Chapter2":
                    SceneController.instance.toLevel(4);
                    break;
                case "BossFight":
                    SceneController.instance.toLevel(5);
                    break;
                default:
                    break;
            }
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
