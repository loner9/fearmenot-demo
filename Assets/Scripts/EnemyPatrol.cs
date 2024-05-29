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
    [HideInInspector] public int cue = 1;
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.right.normalized;
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit)
        {
            patrol();
        }
    }

    // private void FixedUpdate()
    // {
    //     hit = Physics2D.Raycast(transform.position, direction, 0.3f, playerMask);
    //     Debug.DrawRay(transform.position, direction * 0.3f, Color.red);

    //     if (hit.collider != null)
    //     {
    //         isHit = true;
    //     }
    // }

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

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        if (cue == 1)
        {
            direction = (transform.right * -1).normalized;
        }
        else if (cue == -1)
        {
            direction = transform.right.normalized;
        }
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

                Invoke("flip", 0.31f);
                StartCoroutine(onHitEffect(0.3f));

                PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
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

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.8f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.8f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
