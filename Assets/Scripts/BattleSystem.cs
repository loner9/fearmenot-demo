using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Cinemachine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;
    public Text messageText;
    public GameObject player;
    public GameObject enemy;
    unit playerUnit;
    unit enemyUnit;
    bool isSecondPhase;
    bool isDoneChanging;
    PlayerHealth health;
    float defaultEvadeChance;
    Animator playerAnim;
    Animator enemyAnim;
    [SerializeField] GameObject itemPanel;
    int apelAmount, drinkAmount, hp, maxHp;

    CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playerAnim = player.GetComponent<Animator>();
        enemyAnim = enemy.GetComponent<Animator>();
        state = BattleState.START;
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        apelAmount = gameController.apelAmount;
        drinkAmount = gameController.drinkAmount;
        Debug.Log("apel " + apelAmount);
        setGame();
    }

    public void setGame()
    {
        playerUnit = player.GetComponent<unit>();
        enemyUnit = enemy.GetComponent<unit>();

        health = GameObject.Find("PlayerHealth").GetComponent<PlayerHealth>();
        hp = health.getHealth();
        maxHp = health.getMaxHealth();
        playerUnit.setMaxHealth(maxHp);
        playerUnit.setHealth(hp);

        enemyUnit.setMaxHealth(20);
        enemyUnit.setHealth(20);

        defaultEvadeChance = playerUnit.evadeChance;

        Debug.Log(playerUnit.getHealth() + " " + enemyUnit.getHealth());

        playerHealthText.text = playerUnit.getHealth() + "/" + playerUnit.getMaxHealth();
        enemyHealthText.text = enemyUnit.getHealth() + "/" + enemyUnit.getMaxHealth();

        state = BattleState.PLAYERTURN;

        playerTurn();
    }

    public void resetGame()
    {
        state = BattleState.START;
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        apelAmount = gameController.apelAmount;
        drinkAmount = gameController.drinkAmount;
        setGame();
    }

    public void toMainMenu()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    private void playerTurn()
    {
        enemyHealthText.text = enemyUnit.getHealth() + "/" + enemyUnit.getMaxHealth();
        playerUnit.evadeChance = defaultEvadeChance;
        // messageText.text = "Choose an action";
    }

    public void onAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    public void onEvadeButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerEvade());
    }

    public void onItemButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        itemPanel.SetActive(true);
        TextMeshProUGUI countA = itemPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI countB = itemPanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        countA.text = "x" + apelAmount;
        countB.text = "x" + drinkAmount;

    }

    public void onApelButton()
    {
        if (apelAmount <= 0)
        {
            return;
        }
        StartCoroutine(ApelConsumed());
    }

    public void onDrinkButton()
    {
        if (apelAmount <= 0)
        {
            return;
        }
        StartCoroutine(DrinkConsumed());
    }

    public void onSunShardBtn()
    {
        if (isDoneChanging)
        {
            StartCoroutine(sunAction());
        }
    }

    private IEnumerator ApelConsumed()
    {
        messageText.text = "Miaw merasa lebih sehat!!!";
        playerUnit.setHealth(playerUnit.getHealth() + 3);
        apelAmount--;
        itemPanel.SetActive(false);

        yield return new WaitForSeconds(2f);
        playerHealthText.text = playerUnit.getHealth() + "/" + playerUnit.getMaxHealth();

        state = BattleState.ENEMYTURN;
        StartCoroutine(enemyTurn());
    }

    private IEnumerator DrinkConsumed()
    {
        playerUnit.evadeChance = 0.5f;
        playerUnit.setHealth(playerUnit.getHealth() + 1);
        messageText.text = "Miaw merasa lebih segar!!!";
        drinkAmount--;
        itemPanel.SetActive(false);

        yield return new WaitForSeconds(2f);
        playerHealthText.text = playerUnit.getHealth() + "/" + playerUnit.getMaxHealth();

        state = BattleState.ENEMYTURN;
        StartCoroutine(enemyTurn());
    }

    private IEnumerator sunAction()
    {
        bool isDead = false;

        impulseSource.GenerateImpulse();
        isDead = enemy.GetComponent<unit>().takeDamage(999);
        itemPanel.SetActive(false);

        messageText.text = "Fragmen Matahari mengubah Monster itu menjadi butiran debu!!!";

        yield return new WaitForSeconds(2f);

        enemyHealthText.text = enemyUnit.getHealth() + "/" + enemyUnit.getMaxHealth();

        playerAnim.Play("idle");

        if (isDead)
        {
            if (isDoneChanging)
            {
                enemyAnim.Play("dead2");
            }
            else
            {
                enemyAnim.Play("dead");
            }

            state = BattleState.WON;
            endBattle();
        }
    }

    private IEnumerator PlayerAttack()
    {
        // float rand = Random.Range(0.0f, 1.0f);
        var random = new Random(Guid.NewGuid().GetHashCode());

        bool isDead = false;

        double rand = random.NextDouble();

        if (rand < playerUnit.hitChance)
        {
            if (random.NextDouble() < enemyUnit.evadeChance)
            {
                messageText.text = "Serangan Miaw Meleset!!";
                Debug.Log("evade after hitchance");
            }
            else
            {
                playerAnim.Play("attack");
                if (isDoneChanging)
                {
                    enemyAnim.Play("hit2");
                }
                else
                {
                    enemyAnim.Play("hit");
                }
                impulseSource.GenerateImpulse();
                isDead = enemy.GetComponent<unit>().takeDamage(playerUnit.damage);
                Debug.Log("hit is " + rand + ", " + playerUnit.damage);
                messageText.text = "Miaw Menyerang dengan sepenuh kekuatan!";
            }

        }
        else
        {
            playerAnim.Play("idle");
            Debug.Log("miss is " + rand);
            messageText.text = "Miaw terlalu grogi, serangannya meleset";
        }

        itemPanel.SetActive(false);
        yield return new WaitForSeconds(2f);

        enemyHealthText.text = enemyUnit.getHealth() + "/" + enemyUnit.getMaxHealth();

        playerAnim.Play("idle");

        if (isDead)
        {
            if (isDoneChanging)
            {
                enemyAnim.Play("dead2");
            }
            else
            {
                enemyAnim.Play("dead");
            }

            state = BattleState.WON;
            endBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(enemyTurn());
        }
    }

    IEnumerator PlayerEvade()
    {
        playerUnit.evadeChance = 0.85f;
        messageText.text = "Miaw menyiapkan diri untuk serangan";
        itemPanel.SetActive(false);

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(enemyTurn());
    }
    IEnumerator enemyTurn()
    {
        var random = new Random(Guid.NewGuid().GetHashCode());
        bool isDead = false;
        playerHealthText.text = playerUnit.getHealth() + "/" + playerUnit.getMaxHealth();

        if (enemyUnit.getHealth() <= 12)
        {
            isSecondPhase = true;
        }

        if (!isSecondPhase)
        {
            // first phase action
            if (random.NextDouble() < 0.9f)
            {
                isDead = enemyAttacking();
            }
            else
            {
                enemyHeal();
            }

        }
        else
        {
            if (!isDoneChanging)
            {
                enemySwitchPhase();
            }
            else
            {
                enemyUnit.evadeChance = 0.3f;
                if (random.NextDouble() < 0.5f)
                {
                    isDead = enemyAttacking();
                }
                else
                {
                    enemyEvade();
                }
            }
            // second phase action

        }

        yield return new WaitForSeconds(2f);

        playerHealthText.text = playerUnit.getHealth() + "/" + playerUnit.getMaxHealth();

        playerAnim.Play("idle");
        if (isDoneChanging)
        {
            enemyAnim.Play("idle2");
        }
        else
        {
            enemyAnim.Play("idle");
        }

        if (isDead)
        {
            playerAnim.Play("dead");
            if (playerUnit.getHealth() < 0)
            {
                // playerUnit.setHealth(0);
            }
            state = BattleState.LOST;
            endBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            playerTurn();
        }
    }

    public bool enemyAttacking()
    {
        var random = new Random(Guid.NewGuid().GetHashCode());

        bool isDead = false;

        if (random.NextDouble() < enemyUnit.hitChance)
        {
            if (random.NextDouble() < playerUnit.evadeChance)
            {
                messageText.text = "Serangan Monster berhasil Miaw hindari!!";
                Debug.Log("evade after hitchance");
            }
            else
            {
                playerAnim.Play("hurt");
                if (isDoneChanging)
                {
                    enemyAnim.Play("attack2");
                }
                else
                {
                    enemyAnim.Play("attack");
                }
                isDead = player.GetComponent<unit>().takeDamage(enemyUnit.damage);
                messageText.text = "Monster menyerang Miaw!!!";
                impulseSource.GenerateImpulse();
            }

        }
        else
        {
            messageText.text = "Serangan Monster dihindari oleh Miaw!!";
            Debug.Log("evade in else");
        }
        return isDead;
    }

    private void enemyHeal()
    {
        Debug.Log("enemy hp " + enemyUnit.getHealth());
        messageText.text = "Monster melakukan heal";
        enemyUnit.setHealth(enemyUnit.getHealth() + 2);
        Debug.Log("enemy hp after " + enemyUnit.getHealth());

        enemyHealthText.text = enemyUnit.getHealth() + "/" + enemyUnit.getMaxHealth();
    }

    private void enemyEvade()
    {
        messageText.text = "Monster itu melakukan manuver menghindar";
        enemyUnit.evadeChance = 0.85f;
    }

    private void enemySwitchPhase()
    {
        enemyAnim.Play("transition");
        messageText.text = "Monster itu mulai berganti wujud!";
        enemyUnit.evadeChance = 0.9f;
        isDoneChanging = true;
    }

    private void endBattle()
    {
        if (state == BattleState.WON)
        {
            PlayerPrefs.DeleteAll();
            SceneController.instance.nextLevel();
        }
        else if (state == BattleState.LOST)
        {
            PauseMenu pause = GameObject.Find("GameManager").GetComponent<PauseMenu>();
            pause.gameOver();
        }
    }

    public void apelTxt()
    {
        messageText.text = "HP +3";
    }

    public void drinkTxt()
    {
        messageText.text = "HP +1 dan boost kesempatan menghindar";
    }

    public void sunTxt()
    {
        messageText.text = "Lemahkan Monster untuk menggunakan item ini";
        if (isDoneChanging)
        {
            messageText.text = "Cepat, gunakan item ini!!!";
        }
    }

    public void txtDef()
    {
        messageText.text = "Pilih aksi...";
    }
}
