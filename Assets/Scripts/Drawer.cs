using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    public int trigger = 1;
    PlayerControl player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if ((player.interact == interactState.READY) && (trigger != 0))
            {
                // if (trigger != 0)
                // {
                player.interact = interactState.NOT;
                trigger--;
                PauseMenu pauseMenu = GameObject.Find("GameManager").GetComponent<PauseMenu>();
                pauseMenu.showBtnDrawer();
                // }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetType() == typeof(CapsuleCollider2D))
        {
            player = other.GetComponent<PlayerControl>();
            GetComponent<BoxCollider2D>().enabled = false;
            bool isIdle = player.isIdle;
            player.doInteractAct = true;
            if (isIdle)
            {
                Invoke("show", 0.1f);
            }
        }

        // other.transform.Find("interactPop").gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetType() == typeof(CapsuleCollider2D))
        {
            player = other.GetComponent<PlayerControl>();
            player.doInteractAct = false;
            player.interact = interactState.NOT;
            GetComponent<BoxCollider2D>().enabled = true;
            Invoke("hide", 0.1f);
        }
    }

    private void show()
    {
        transform.Find("interactPop").gameObject.SetActive(true);
    }

    private void hide()
    {
        transform.Find("interactPop").gameObject.SetActive(false);
    }
}
