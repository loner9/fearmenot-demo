using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [HideInInspector] public float horizontalMovement;

    float jumpPower = 5f;
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    [HideInInspector] public bool IsFacingRight;
    private CameraFollowObject cameraFollowObject;
    public GameObject cameraFollowGo;
    public Animator animator;
    private bool isLanding = false;
    private BoxCollider2D hitbox;
    private bool isCoolDown = false;
    private float cdTime = 0.5f;

    public float kbForce;
    public float KbCounter;
    public float KbTotalTime;
    public bool knockFromRight;


    // Start is called before the first frame update
    void Start()
    {
        cameraFollowObject = cameraFollowGo.GetComponent<CameraFollowObject>();
        IsFacingRight = true;
        hitbox = transform.Find("AttackHitbox").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);

    }

    private void FixedUpdate()
    {
        if (KbCounter <= 0)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            animator.SetBool("isHit", false);
        }
        else
        {
            if (knockFromRight)
            {
                rb.velocity = new Vector2(-kbForce, 2);
            }
            else if (!knockFromRight)
            {
                rb.velocity = new Vector2(kbForce, 2);
            }
            // else if(isCoolDown)
            // {
            //     rb.velocity = Vector2.zero;
            // }
            animator.SetBool("isHit", true);
            KbCounter -= Time.deltaTime;
        }

        if (isCoolDown)
        {
            rb.velocity = Vector2.zero;
        }

        if (horizontalMovement > 0 || horizontalMovement < 0)
        {
            turnCheck();
        }
    }

    public void move(InputAction.CallbackContext context)
    {

        horizontalMovement = context.ReadValue<Vector2>().x;

    }

    public void jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                animator.SetTrigger("jump");
            }
        }
        else if (context.canceled && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            animator.SetTrigger("jump");
        }
    }

    public void attack(InputAction.CallbackContext context)
    {
        if (isGrounded() && isCoolDown == false)
        {
            animator.SetTrigger("attacking");
            Invoke("ActivateHitbox", 0.1f);
            Invoke("DeactivateHitbox", 0.4f);
            StartCoroutine(CoolD());
        }
    }

    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            isLanding = true;
            if (isLanding)
            {
                animator.SetTrigger("landing");
            }
            return true;
        }
        isLanding = false;
        return false;
    }

    private IEnumerator CoolD()
    {
        isCoolDown = true;
        yield return new WaitForSeconds(cdTime);
        isCoolDown = false;
    }

    private void turnCheck()
    {
        if (horizontalMovement > 0 && !IsFacingRight)
        {
            turn();
        }
        else if (horizontalMovement < 0 && IsFacingRight)
        {
            turn();
        }
    }

    private void turn()
    {
        if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.y);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            cameraFollowObject.callTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.y);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            cameraFollowObject.callTurn();
        }
    }

    void ActivateHitbox()
    {
        hitbox.gameObject.SetActive(true);
    }

    void DeactivateHitbox()
    {
        hitbox.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }
}
