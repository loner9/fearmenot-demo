using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    private CapsuleCollider2D capsuleCollider2D;
    [SerializeField] float speed;
    bool isHit = false;
    RaycastHit2D hit;
    public LayerMask playerMask;
    Vector3 direction;
    Animator animator;
    [HideInInspector] public bool isDead;
    [HideInInspector] public int cue = 1;
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.right.normalized;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        currentPoint = pointB.transform;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!isHit)
            {
                patrol();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
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
            currentPoint = pointA.transform;
            flip();
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.8f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
            flip();
        }

    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        cue *= -1;
        localScale.x = cue;
        transform.localScale = localScale;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Char")
        {
            // Debug.Log(collision.collider.GetType());
            if (collision.collider.GetType() == typeof(CapsuleCollider2D))
            {
                int val = 1;
                if (currentPoint == pointB.transform)
                {
                    val = -1;
                    currentPoint = pointA.transform;
                }
                else if (currentPoint == pointA.transform)
                {
                    val = 1;
                    currentPoint = pointB.transform;
                }

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

                // Invoke("flip", 0.5f);
                StartCoroutine(onHitEffect(0.3f));

                PlayerHealth playerHealth = GameObject.Find("PlayerHealth").GetComponent<PlayerHealth>();
                playerHealth.takeDamage(2, new Vector2(val, 0));
            }
        }
    }

    private IEnumerator onHitEffect(float delay)
    {
        isHit = true;

        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        capsuleCollider2D.enabled = false;
        yield return new WaitForSeconds(delay);
        capsuleCollider2D.enabled = true;
        isHit = false;
        Invoke("flip", 0.5f);

    }

    public void disable()
    {
        rb.velocity = Vector2.zero;
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
}