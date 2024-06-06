using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyPatrolDive : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    [SerializeField] float speed;
    bool isColliding = false;
    [HideInInspector] public bool isDead = false;
    bool isFacingRight;
    RaycastHit2D hit;
    Vector3 direction;
    public LayerMask playerMask;
    BoxCollider2D boxCollider;
    Animator animator;
    [HideInInspector] public int cue = 1;
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();
    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.right - transform.up).normalized;
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!isColliding)
            {
                patrol();
            }
            else
            {
                dive();
            }
        }

    }

    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(transform.position, direction, 6f, playerMask);
        Debug.DrawRay(transform.position, direction * 6f, Color.red);

        if (hit.collider != null)
        {
            isColliding = true;
        }
    }

    void patrol()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.8f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.8f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }
    }

    void dive()
    {
        // var direction = (transform.right - transform.up).normalized;
        rb.velocity = direction * 5;
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        if (cue == 1)
        {
            direction = ((transform.right * -1) - transform.up).normalized;
        }
        else if (cue == -1)
        {
            direction = (transform.right - transform.up).normalized;
        }
        cue *= -1;
        localScale.x = cue;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("Ground") || collision.collider.name == "Char")
        {
            // Debug.Log(collision.collider.GetType());
            if (collision.collider.GetType() == typeof(CapsuleCollider2D))
            {
                PlayerControl player = collision.collider.GetComponent<PlayerControl>();
                player.KbCounter = player.KbTotalTime;
                if (collision.transform.position.x <= transform.position.x)
                {
                    player.knockFromRight = true;
                }
                else if (collision.transform.position.x > transform.position.x)
                {
                    player.knockFromRight = false;
                }

                PlayerHealth playerHealth = GameObject.Find("PlayerHealth").GetComponent<PlayerHealth>();
                playerHealth.takeDamage(1, new Vector2(cue, 0f));
            }
            // boxCollider.enabled = false;
            isDead = true;
            disable();
        }
    }

    public void disable()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0.0f;
        boxCollider.enabled = false;
        rb.isKinematic = true;
        animator.Play("dead");
    }
    void begone()
    {
        dropItems();
        Destroy(gameObject);
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
            Instantiate(loot, transform.position, Quaternion.identity);
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(pointA.transform.position, 0.8f);
    //     Gizmos.DrawWireSphere(pointB.transform.position, 0.8f);
    //     Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    // }
}
