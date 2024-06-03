using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crates : MonoBehaviour
{
    public Sprite[] cratesSprite;
    int trigger = 1;
    PlayerControl player;
    GameObject spawnPoint;
    public List<LootItem> lootTable = new List<LootItem>();
    public GameObject exclusiveItem;
    public bool isExclusiveItems = false;

    private void Awake()
    {
        Sprite ini = cratesSprite[Random.Range(0, cratesSprite.Length - 1)];
        GetComponent<SpriteRenderer>().sprite = cratesSprite[Random.Range(0, cratesSprite.Length - 1)];
        spawnPoint = transform.Find("SpawnPoint").gameObject;
        Debug.Log("sprite :" + ini.name);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (player.interact == interactState.READY)
            {
                if (trigger != 0)
                {
                    trigger--;
                    Debug.Log("item spawned");
                    if (!isExclusiveItems)
                    {
                        dropItems();
                    }
                    else
                    {
                        Instantiate(exclusiveItem, spawnPoint.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetType() == typeof(CapsuleCollider2D))
        {
            player = other.GetComponent<PlayerControl>();
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
            Invoke("hide", 0.1f);
        }
    }

    void dropItems()
    {
        foreach (LootItem item in lootTable)
        {
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                instatiateLoot(item.itemPrefab);
            }
            break;
        }
    }
    void instatiateLoot(GameObject loot)
    {
        if (loot)
        {
            Instantiate(loot, spawnPoint.transform.position, Quaternion.identity);
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
