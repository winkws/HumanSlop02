using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpHeight;

    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] int wallBouncesForDashReset;

    [SerializeField] float minWallJumpTime;
    [SerializeField] float maxWallJumpTime;
    [SerializeField] float wallJumpHeight;

    [SerializeField] float coyoteTime;

    [SerializeField] float jumpBufferTime;

    [SerializeField] float groundDamping;
    [SerializeField] float airDamping;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheckLeft;
    [SerializeField] Transform wallCheckRight;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    InputAction movementAction;
    InputAction jumpAction;
    InputAction dashAction;

    float dashTimer = 0f;
    float wallJumpTimer = 0f;
    float coyoteTimer = 0f;
    float timeSinceJump = 0f;
    float jumpBufferTimer = 0f;

    int wallJumps = 0;

    bool dashAvailable = false;
    bool dashing = false;
    bool wallJumping = false;
    bool wallHugging = false;

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
            rb.linearDamping = airDamping;
            rb.linearVelocity = Vector2.zero;
        }

        wallJumpTimer += Time.deltaTime;
        if (wallJumping && wallJumpTimer > minWallJumpTime && !jumpAction.IsPressed() || wallJumping && wallJumpTimer > maxWallJumpTime)
        {
            rb.linearVelocityY = 0;
            wallJumping = false;
        }
        else if (wallJumping) return;

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

        if (jumpAction.WasPressedThisFrame() || jumpBufferTimer > 0 && jumpAction.IsPressed() && timeSinceJump > 0.15f)
        {
            if (wallHugging)
            {
                WallJump(WallCheck());
                return;
            }

            if (GroundCheck() || coyoteTimer > 0)
            {
                Jump();
            }
        }

        if (jumpAction.WasPressedThisFrame())
        {
            jumpBufferTimer = jumpBufferTime;
        }

        jumpBufferTimer -= Time.deltaTime;

        if (GroundCheck() && timeSinceJump > 0.05f)
        {
            dashAvailable = true;
            coyoteTimer = coyoteTime;
            rb.linearDamping = groundDamping;
            animator.SetBool("Jumping", false);
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            animator.SetBool("Jumping", true);
            rb.linearDamping = airDamping;
        }
        
        if (movement.x == 0)
        {
            animator.SetInteger("Movement", 0);
        }

        if (movement == Vector2.zero) return;

        if (dashAction.WasPressedThisFrame() && dashAvailable && !wallHugging)
        {
            Dash(movement);
            return;
        }

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
        dashAvailable = false;

        dashTimer = dashTime;

        rb.gravityScale = 0;
        rb.linearDamping = 0;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(movement.x * dashSpeed, movement.y * dashSpeed), ForceMode2D.Impulse);

        animator.SetBool("Dashing", true);
    }

    void WallJump(int movement)
    {
        wallJumpTimer = 0;
        wallJumping = true;

        sr.flipX = movement > 0;

        timeSinceJump = 0f;
        jumpBufferTimer = 0f;

        wallJumps++;

        if(wallJumps >= wallBouncesForDashReset)
        {
            dashAvailable = true;
            wallJumps = 0;
        }

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
        Collider2D[] collider = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(0.35f, 0.1f), 0f);
        return collider.Where(x => x.gameObject.CompareTag("Ground")).ToList().Count != 0;
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