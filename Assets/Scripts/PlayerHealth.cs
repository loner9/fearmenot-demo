using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 7;
    private int currentHealth;
    private KnockBack knockBack;
    private PlayerControl playerControl;
    public GameObject healthRateContainer;
    public GameObject player;
    HealthRate healthRate;

    // Start is called before the first frame update

    private void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        if (!PlayerPrefs.GetString("scene", "").Equals("BossFight"))
        {
            healthRate = GameObject.Find("Particle System").GetComponent<HealthRate>();
            player = GameObject.Find("Char");
        }

        currentHealth = maxHealth;
        Debug.Log("current " + currentHealth);
    }

    void Start()
    {
        if (!PlayerPrefs.GetString("scene").Equals("BossFight"))
        {
            knockBack = player.GetComponent<KnockBack>();
            playerControl = player.GetComponent<PlayerControl>();
        }
    }

    public void takeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;
        changeHeartRate();
        if (currentHealth <= 0)
        {
            //player dead
            playerControl.isDead();
            Debug.Log("is dead");
        }
    }

    public void heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        changeHeartRate();
    }

    void changeHeartRate()
    {
        if (currentHealth >= 5)
        {
            healthRate.setHealtLevel("high");
        }
        else if (currentHealth >= 3)
        {
            healthRate.setHealtLevel("mid");
        }
        else
        {
            healthRate.setHealtLevel("low");
        }
    }

    public void initialState()
    {
        currentHealth = maxHealth;
        changeHeartRate();
    }

    public void setHealth(int amount)
    {
        currentHealth = amount;
    }

    public int getHealth()
    {
        return currentHealth;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }
}
