using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public enum interactState { READY, NOT }
public class PlayerControl : MonoBehaviour
{
    PlayerInput playerInput;
    public Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    [HideInInspector] public float horizontalMovement;

    float jumpPower = 6f;
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    public bool IsFacingRight;
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
    int currentApel;
    int currentDrink;
    bool lanterCollected;
    bool SunShardCollected;
    bool CompassCollected;
    GameObject playerLight;
    GameController gameController;
    PlayerHealth health;

    [HideInInspector] public bool isIdle = false;

    [HideInInspector] public bool doInteractAct = false;

    public interactState interact;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        health = GameObject.Find("PlayerHealth").GetComponent<PlayerHealth>();
    }

    // Start is called before the first frame update
    void Start()
    {
        stateA();
        cameraFollowObject = cameraFollowGo.GetComponent<CameraFollowObject>();
        hitbox = transform.Find("AttackHitbox").GetComponent<BoxCollider2D>();
        interact = interactState.NOT;
        playerLight = transform.Find("FlashLight").gameObject;
        if (gameController.isLanternCollected)
        {
            playerLight.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        isIdle = rb.velocity.magnitude < 0.1 ? true : false;
        if (gameController.isLanternCollected)
        {
            playerLight.gameObject.SetActive(true);
        }
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

    public void interactAction(InputAction.CallbackContext context)
    {
        if (doInteractAct)
        {
            interact = interactState.READY;
        }
    }

    public void decreaseHealth(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            health.takeDamage(1, Vector2.left);
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

    public void lanternOn()
    {
        playerLight.gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }

    public void isDead()
    {
        playerInput.DeactivateInput();
        animator.SetTrigger("dead");
    }

    public void stateA()
    {
        gameController.initialPos = transform.position;
        currentApel = gameController.apelAmount;
        currentDrink = gameController.drinkAmount;
        lanterCollected = gameController.isLanternCollected;
        SunShardCollected = gameController.isSunShardCollected;
        CompassCollected = gameController.isCompassCollected;
    }

    public void stateB()
    {
        gameController.apelAmount = currentApel;
        gameController.drinkAmount = currentDrink;
        gameController.isLanternCollected = lanterCollected;
        gameController.isSunShardCollected = SunShardCollected;
        gameController.isCompassCollected = CompassCollected;
    }

    public void resetState()
    {
        playerInput.ActivateInput();
        animator.Rebind();
        animator.Update(0f);
        transform.position = gameController.initialPos;
        stateB();
        health.initialState();
    }
}
