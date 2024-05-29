using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Cinemachine;

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

    float defaultEvadeChance;

    CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        state = BattleState.START;
        setGame();
    }

    public void setGame()
    {
        playerUnit = player.GetComponent<unit>();
        enemyUnit = enemy.GetComponent<unit>();

        defaultEvadeChance = playerUnit.evadeChance;

        playerHealthText.text = playerUnit.currentHP + "/" + playerUnit.maxHP;
        enemyHealthText.text = enemyUnit.currentHP + "/" + enemyUnit.maxHP;

        state = BattleState.PLAYERTURN;

        playerTurn();
    }

    private void playerTurn()
    {
        enemyHealthText.text = enemyUnit.currentHP + "/" + enemyUnit.maxHP;
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

    private IEnumerator PlayerAttack()
    {
        // float rand = Random.Range(0.0f, 1.0f);
        var random = new Random(Guid.NewGuid().GetHashCode());

        bool isDead = false;

        double rand = random.NextDouble();

        if (rand < playerUnit.hitChance)
        {
            impulseSource.GenerateImpulse();
            isDead = enemy.GetComponent<unit>().takeDamage(playerUnit.damage);
            Debug.Log("hit is " + rand);
            messageText.text = "Miaw is Attacking";
        }
        else
        {
            Debug.Log("miss is " + rand);
            messageText.text = "Miaw is too nerveous, hit attack is missed";
        }

        yield return new WaitForSeconds(2f);

        enemyHealthText.text = enemyUnit.currentHP + "/" + enemyUnit.maxHP;

        if (isDead)
        {
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
        messageText.text = "Miaw is preparing for enemy attack!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(enemyTurn());
    }
    IEnumerator enemyTurn()
    {
        var random = new Random(Guid.NewGuid().GetHashCode());
        bool isDead = false;
        playerHealthText.text = playerUnit.currentHP + "/" + playerUnit.maxHP;

        if (enemyUnit.currentHP <= 12)
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

        playerHealthText.text = playerUnit.currentHP + "/" + playerUnit.maxHP;

        if (isDead)
        {
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
                messageText.text = "Boss attack dodged by Miaw!";
                Debug.Log("evade after hitchance");
            }
            else
            {
                isDead = player.GetComponent<unit>().takeDamage(enemyUnit.damage);
                messageText.text = "Boss is Attacking";
                impulseSource.GenerateImpulse();
            }

        }
        else
        {
            messageText.text = "Boss attack dodged by Miaw!";
            Debug.Log("evade in else");
        }
        return isDead;
    }

    private void enemyHeal()
    {
        Debug.Log("enemy hp " + enemyUnit.currentHP);
        messageText.text = "Boss is healing!";
        enemyUnit.currentHP += 2;
        Debug.Log("enemy hp after " + enemyUnit.currentHP);

        enemyHealthText.text = enemyUnit.currentHP + "/" + enemyUnit.maxHP;
    }

    private void enemyEvade()
    {
        messageText.text = "Boss is preparing an evade manuver";
        enemyUnit.evadeChance = 0.85f;
    }

    private void enemySwitchPhase()
    {
        messageText.text = "Boss is changing into another shape!";
        isDoneChanging = true;
    }

    private void endBattle()
    {
        if (state == BattleState.WON)
        {

        }
        else if (true)
        {

        }
    }
}
