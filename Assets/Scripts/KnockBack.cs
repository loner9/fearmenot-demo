using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float knockBackTime = 0.2f;
    public float hitDirectionForce = 10f;
    public float constForce = 5f;
    public float inputForce = 7.5f;
    Rigidbody2D rb;
    private Coroutine knockBackCoroutine;
    private Animator animator;

    public bool isBeingKnockedUp { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public IEnumerator KnockBackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        isBeingKnockedUp = true;
        animator.SetBool("isHit", isBeingKnockedUp);

        Vector2 hitForce;
        Vector2 constantForce;
        Vector2 knockBackForce;
        Vector2 combinedForce;

        hitForce = hitDirection * hitDirectionForce;
        constantForce = constantForceDirection * constForce;

        float elapsedime = 0f;
        while (elapsedime < knockBackTime)
        {
            elapsedime += Time.fixedDeltaTime;

            knockBackForce = hitForce + constantForce;

            if (inputDirection != 0)
            {
                combinedForce = knockBackForce + new Vector2(inputDirection * inputForce, 0f);
            }
            else
            {
                combinedForce = knockBackForce;
            }

            rb.velocity = new Vector2(combinedForce.x, combinedForce.y);

            yield return new WaitForFixedUpdate();
        }

        isBeingKnockedUp = false;
        animator.SetBool("isHit", isBeingKnockedUp);
    }

    public void callKnockBack(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        knockBackCoroutine = StartCoroutine(KnockBackAction(hitDirection, constantForceDirection, inputDirection));
    }
}
