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

    InputAction movementAction;
    InputAction jumpAction;
    InputAction dashAction;

    float dashCooldownTimer = 0f;
    float dashTimer = 0f;
    float wallJumpTimer = 0f;
    bool dashing = false;
    bool wallJumping = false;

    float gravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        movementAction = InputSystem.actions.FindAction("movement");
        jumpAction = InputSystem.actions.FindAction("jump");
        dashAction = InputSystem.actions.FindAction("dash");

        gravity = rb.gravityScale;
    }

    void Update()
    {
        if (dashing && dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
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

        if (jumpAction.WasPressedThisFrame())
        {
            if (GroundCheck())
            {
                Jump();
            }

            int wallCheck = WallCheck();
            if (!GroundCheck() && wallCheck != 0)
            {
                WallJump(wallCheck);
                return;
            }
        }

        Vector2 movement = movementAction.ReadValue<Vector2>();

        if (GroundCheck())
        {
            rb.linearDamping = 10f;
        }
        else
        {
            rb.linearDamping = damping;
        }
        
        //if (movement.x == 0) rb.linearVelocityX = 0;
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
    }

    void WallJump(int movement)
    {
        wallJumpTimer = wallJumpTime;
        wallJumping = true;

        sr.flipX = movement > 0;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(-movement * wallJumpHeight, wallJumpHeight), ForceMode2D.Impulse);
    }

    void Move(Vector2 movement)
    {
        rb.linearVelocityX = movement.x * movementSpeed;
        sr.flipX = movement.x < 0;
    }

    bool GroundCheck()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(groundCheck.position, Vector2.down, 0.25f);
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