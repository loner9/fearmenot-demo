using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextLevel : MonoBehaviour
{
    PlayerControl player;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "Char")
        {
            player = other.GetComponent<PlayerControl>();
            Invoke("nexx", 0.2f);

        }
    }

    void nexx()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        PlayerPrefs.SetInt("apelAmount", gameController.apelAmount);
        PlayerPrefs.SetInt("drinkAmount", gameController.drinkAmount);
        PlayerPrefs.SetString("lantern", gameController.isLanternCollected.ToString());
        PlayerPrefs.SetString("sun", gameController.isSunShardCollected.ToString());
        PlayerPrefs.SetString("compass", gameController.isCompassCollected.ToString());

        SceneController.instance.nextLevel();
    }
}
