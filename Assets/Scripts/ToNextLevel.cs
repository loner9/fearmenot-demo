using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextLevel : MonoBehaviour
{
    PlayerControl player;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (other.gameObject.name == "Char")
        {
            player = other.GetComponent<PlayerControl>();
            if (PlayerPrefs.GetString("scene") == "Chapter2")
            {
                if (gameController.isCompassFixed)
                {
                    Invoke("nexx", 0.2f);
                }
            }
            else
            {
                Invoke("nexx", 0.2f);
            }
        }
    }

    void nexx()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        PlayerHealth health = GameObject.Find("PlayerHealth").GetComponent<PlayerHealth>();
        PlayerPrefs.SetInt("apelAmount", gameController.apelAmount);
        PlayerPrefs.SetInt("drinkAmount", gameController.drinkAmount);
        PlayerPrefs.SetString("lantern", gameController.isLanternCollected.ToString());
        PlayerPrefs.SetString("sun", gameController.isSunShardCollected.ToString());
        PlayerPrefs.SetString("compass", gameController.isCompassCollected.ToString());
        PlayerPrefs.SetInt("health", health.getHealth());
        SceneController.instance.nextLevel();
    }
}
