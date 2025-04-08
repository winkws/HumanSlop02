using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpHeight;

    [SerializeField] float dashCooldown;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;

    [SerializeField] float wallJumpTime;
    [SerializeField] float wallJumpHeight;

    [SerializeField] float damping;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheckLeft;
    [SerializeField] Transform wallCheckRight;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    InputAction movementAction;
    InputAction jumpAction;
    InputAction dashAction;

    float dashCooldownTimer = 0f;
    float dashTimer = 0f;
    float wallJumpTimer = 0f;
    bool dashing = false;
    bool wallJumping = false;
    bool wallHugging = false;

    float timeSinceJump = 0f;

    float gravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        movementAction = InputSystem.actions.FindAction("movement");
        jumpAction = InputSystem.actions.FindAction("jump");
        dashAction = InputSystem.actions.FindAction("dash");

        gravity = rb.gravityScale;
    }

    void Update()
    {
        timeSinceJump += Time.deltaTime;

        if (dashing && dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;

            animator.SetBool("Dashing", false);

            return;
        }
        else if (dashing && dashTimer <= 0)
        {
            dashing = false;
            rb.gravityScale = gravity;
            rb.linearDamping = damping;
            rb.linearVelocity = Vector2.zero;
        }

        if (wallJumping && wallJumpTimer > 0 && jumpAction.IsPressed())
        {
            wallJumpTimer -= Time.deltaTime;
            return;
        }
        else if (wallJumping && wallJumpTimer <= 0 || wallJumping && !jumpAction.IsPressed())
        {
            rb.linearVelocityY = 0;
            wallJumping = false;
        }

        Vector2 movement = movementAction.ReadValue<Vector2>();

        if(WallCheck() != 0 && movement.x == WallCheck())
        {
            wallHugging = true;
            animator.SetBool("WallHugging", true);
        }
        else
        {
            wallHugging = false;
            animator.SetBool("WallHugging", false);
        }

        if (jumpAction.WasPressedThisFrame())
        {
            if (wallHugging)
            {
                WallJump(WallCheck());
                return;
            }

            if (GroundCheck())
            {
                Jump();
            }
        }

        if (GroundCheck() && timeSinceJump > 0.05f)
        {
            rb.linearDamping = 10f;
            animator.SetBool("Jumping", false);
        }
        else
        {
            rb.linearDamping = damping;
        }
        
        if (movement.x == 0)
        {
            animator.SetInteger("Movement", 0);
        }

        if (movement == Vector2.zero) return;

        if (dashAction.WasPressedThisFrame() && dashCooldownTimer <= 0)
        {
            Dash(movement);
            return;
        }
        dashCooldownTimer -= Time.deltaTime;

        if (movement.x == 0) return;

        Move(movement);
    }

    void Jump()
    {
        rb.linearVelocityY = 0;
        rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

        timeSinceJump = 0f;

        animator.SetBool("Jumping", true);
    }

    void Dash(Vector2 movement)
    {
        dashing = true;

        dashCooldownTimer = dashCooldown;
        dashTimer = dashTime;

        rb.gravityScale = 0;
        rb.linearDamping = 0;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(movement.x * dashSpeed, movement.y * dashSpeed), ForceMode2D.Impulse);

        animator.SetBool("Dashing", true);
    }

    void WallJump(int movement)
    {
        wallJumpTimer = wallJumpTime;
        wallJumping = true;

        sr.flipX = movement > 0;

        timeSinceJump = 0f;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(-movement * wallJumpHeight, wallJumpHeight), ForceMode2D.Impulse);

        animator.SetBool("WallHugging", false);
        animator.SetBool("Jumping", true);
    }

    void Move(Vector2 movement)
    {
        rb.linearVelocityX = movement.x * movementSpeed;
        sr.flipX = movement.x < 0;

        animator.SetInteger("Movement", (int)movement.x);
    }

    bool GroundCheck()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(groundCheck.position, Vector2.down, 0.025f);
        return hits.Where(x => x.collider.gameObject.CompareTag("Ground")).ToList().Count != 0;
    }

    int WallCheck()
    {
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(wallCheckLeft.position, Vector2.left, 0.15f);
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(wallCheckRight.position, Vector2.right, 0.15f);

        bool isWallOnLeft = hitsLeft.Where(x => x.collider.gameObject.CompareTag("Ground")).ToList().Count != 0;
        bool isWallOnRight = hitsRight.Where(x => x.collider.gameObject.CompareTag("Ground")).ToList().Count != 0;

        if (isWallOnLeft) return -1;
        if (isWallOnRight) return 1;

        return 0;
    }
}