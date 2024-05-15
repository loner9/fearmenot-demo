using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolDive : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    [SerializeField] float speed;
    bool isColliding = false;
    bool isFacingRight;
    RaycastHit2D hit;
    Vector3 direction;
    public LayerMask playerMask;
    int cue = 1;
    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.right - transform.up).normalized;
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
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
        localScale.x *= cue;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Ground" || collision.collider.name == "Char")
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.8f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.8f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
