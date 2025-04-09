using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Movement parameters
    [Header("Basic movement")]
    [SerializeField] float movementSpeed; // Movement speed of the player
    [SerializeField] float initialJumpHeight; // Initial jump height (force applied on jump)
    [SerializeField] float jumpForce; // Force applied during the jump for maintaining height
    [SerializeField] float minJumpTime; // Minimum time player can hold jump for
    [SerializeField] float maxJumpTime; // Maximum time player can hold jump for

    // Dash parameters
    [Header("Dash")]
    [SerializeField] float dashTime; // Duration of the dash
    [SerializeField] float dashSpeed; // Speed of the dash
    [SerializeField] int wallJumpsRequiredForDashReset; // Number of wall jumps required to reset dash

    // Wall jump parameters
    [Header("Wall jump")]
    [SerializeField] float minWallJumpTime; // Minimum time to hold jump during wall jump
    [SerializeField] float maxWallJumpTime; // Maximum time to hold jump during wall jump
    [SerializeField] float initialWallJumpIniVerticalMovement; // Initial vertical force on wall jump
    [SerializeField] float initialWallJumpHorizontalMovement; // Initial horizontal force on wall jump
    [SerializeField] float verticalWallJumpForce; // Force applied vertically during wall jump
    [SerializeField] float horizontalWallJumpForce; // Force applied horizontally during wall jump

    // Advanced movement parameters
    [Header("Advanced")]
    [SerializeField] float coyoteTime; // Time after falling off the edge where jump is still possible
    [SerializeField] float jumpBufferTime; // Time window for pressing jump after falling off
    [SerializeField] float groundDamping; // Damping when the player is on the ground
    [SerializeField] float airDamping; // Damping when the player is in the air

    // Surface detection
    [Header("Surface detection")]
    [SerializeField] Transform groundCheck; // Transform to check if the player is grounded
    [SerializeField] Transform wallCheckLeft; // Transform to check if the player is near a left wall
    [SerializeField] Transform wallCheckRight; // Transform to check if the player is near a right wall

    // Components and variables for handling movement
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    InputAction movementAction; // Input action for movement
    InputAction jumpAction; // Input action for jump
    InputAction dashAction; // Input action for dash

    Vector2 movement; // The player's movement vector

    // Timers for various actions
    float coyoteTimer = 0f;
    float timeSinceJump = 0f;

    int wallJumps = 0; // Counter for wall jumps

    // States to track different actions
    bool jumping;
    bool dashAvailable = false;
    bool dashing = false;
    bool wallJumping = false;
    bool wallHugging = false;
    bool grounded = false;

    // Gravity variable to reset after dash
    float gravity;

    void Start()
    {
        // Initializing components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Assigning input actions from the Input System
        movementAction = InputSystem.actions.FindAction("movement");
        jumpAction = InputSystem.actions.FindAction("jump");
        dashAction = InputSystem.actions.FindAction("dash");

        // Saving initial gravity scale to restore after dash
        gravity = rb.gravityScale;
    }

    void Update()
    {
        // Skip logic if currently dashing
        if (dashing) return;

        // Get the current movement direction
        movement = movementAction.ReadValue<Vector2>();

        // Update timers
        HandleTimers();

        // Check for surface status (grounded, wall check)
        SurfaceChecks();

        // Handle jump input
        if (jumpAction.WasPressedThisFrame())
        {
            HandleJumpInput();
        }

        // Handle dash input
        if (dashAction.WasPressedThisFrame())
        {
            HandleDashInput();
        }

        // If wall jumping or dashing, skip regular movement logic
        if (wallJumping || dashing) return;

        // Handle regular movement if not jumping or dashing
        HandleMovement();
    }

    void HandleTimers()
    {
        // Increment time since jump
        timeSinceJump += Time.deltaTime;
    }

    void SurfaceChecks()
    {
        // Check if the player is grounded
        if (GroundCheck() && timeSinceJump > 0.05f)
        {
            // Reset wall jumps and enable dash if grounded
            wallJumps = 0;
            dashAvailable = true;
            grounded = true;
            jumping = false;
            coyoteTimer = coyoteTime;
            rb.linearDamping = groundDamping; // Apply ground damping
            animator.SetBool("Jumping", false); // Update animator
        }
        else
        {
            // Decrease coyote timer if not grounded
            coyoteTimer -= Time.deltaTime;
            animator.SetBool("Jumping", true); // Update animator to show jumping
            grounded = false;
            rb.linearDamping = airDamping; // Apply air damping
        }

        // Check for wall collisions on both sides
        int wallCheck = WallCheck();
        if (wallCheck != 0)
        {
            // Player is hugging a wall if moving towards it
            if (movement.x == wallCheck)
            {
                wallHugging = true;
            }
            else
            {
                wallHugging = false;
            }
        }
        else if (wallJumping) wallHugging = false;
        else wallHugging = false;

        animator.SetBool("WallHugging", wallHugging); // Update animator for wall hugging state
    }

    void HandleMovement()
    {
        // If no horizontal movement, reset animation state
        if (movement.x == 0)
        {
            animator.SetInteger("Movement", 0); 
            return;
        }

        // Apply movement force
        Move(movement);
    }

    void HandleJumpInput()
    {
        // If not grounded and not wall hugging, start buffering jump
        if (!grounded && !wallHugging && coyoteTimer <= 0)
        {
            StartCoroutine(BufferJump());
            return;
        }

        // Start wall jump if wall hugging
        if (wallHugging)
        {
            StartCoroutine(WallJump());
            return;
        }

        // Start regular jump
        StartCoroutine(Jump());
    }

    void HandleDashInput()
    {
        // If dash is available and player is moving, start dash
        if (!dashAvailable || movement == Vector2.zero) return;

        StartCoroutine(Dash());
    }

    void Move(Vector2 movement)
    {
        // Set player's horizontal velocity
        rb.linearVelocityX = movement.x * movementSpeed;

        // Flip sprite based on movement direction
        sr.flipX = movement.x < 0;

        // Update movement animation state
        animator.SetInteger("Movement", (int)movement.x);
    }

    // Jump coroutine: applies force over time while jump button is held
    IEnumerator Jump()
    {
        if (jumping) yield break; // Prevent multiple jumps simultaneously

        jumping = true;

        rb.linearVelocityY = 0; // Reset vertical velocity
        timeSinceJump = 0f;
        animator.SetBool("Jumping", true);

        // Apply initial jump force
        rb.AddForce(new Vector2(0, initialJumpHeight), ForceMode2D.Impulse);

        float timeElapsed = 0f;
        while (timeElapsed < maxJumpTime)
        {
            // Break if jump is released after minimum time
            if (dashing || timeElapsed >= minJumpTime && !jumpAction.IsPressed()) break;

            // Apply additional jump force while holding jump button
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    // Wall jump coroutine: handles the movement when bouncing off a wall
    IEnumerator WallJump()
    {
        wallJumping = true;

        sr.flipX = movement.x > 0; // Flip sprite based on jump direction

        timeSinceJump = 0f;

        wallJumps++;

        // Reset dash if enough wall jumps have occurred
        if (wallJumps >= wallJumpsRequiredForDashReset)
        {
            dashAvailable = true;
            wallJumps = 0;
        }

        rb.linearVelocity = Vector2.zero; // Reset velocity

        animator.SetBool("WallHugging", false);
        animator.SetBool("Jumping", true);

        Vector2 wallJumpDirection = -movement;
   
        // Apply wall jump force in opposite direction
        rb.AddForce(new Vector2(wallJumpDirection.x * initialWallJumpHorizontalMovement, initialWallJumpIniVerticalMovement), ForceMode2D.Impulse);

        float timeElapsed = 0f;
        while (timeElapsed < maxWallJumpTime)
        {
            // Continue wall jump force until time expires or jump button is released
            if (dashing || timeElapsed >= minWallJumpTime && !jumpAction.IsPressed()) break;

            rb.AddForce(new Vector2(wallJumpDirection.x * horizontalWallJumpForce, verticalWallJumpForce), ForceMode2D.Force);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        wallJumping = false;
    }

    // Dash coroutine: handles the dash logic
    IEnumerator Dash()
    {
        dashing = true;
        dashAvailable = false;

        sr.flipX = movement.x < 0; // Flip sprite based on jump direction

        // Disable gravity during dash
        rb.gravityScale = 0;
        rb.linearDamping = 0;

        // Set the player's dash velocity
        rb.linearVelocity = movement.normalized * dashSpeed;

        animator.SetInteger("Movement", (int)movement.x);
        animator.SetBool("Dashing", true);

        yield return new WaitForSeconds(dashTime);

        // Reset after dash finishes
        dashing = false;
        rb.gravityScale = gravity;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("Dashing", false);
    }

    // Buffer jump coroutine: allows for a brief window to press jump after falling off an edge
    IEnumerator BufferJump()
    {
        float timeElapsed = 0f;
        while (timeElapsed < jumpBufferTime)
        {
            // If grounded and jump is pressed, perform a buffered jump
            if (grounded && jumpAction.IsPressed() && !jumping)
            {
                animator.SetTrigger("BufferJump");
                StartCoroutine(Jump());
            }

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    // Check if the player is grounded using overlap box
    bool GroundCheck()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(0.35f, 0.1f), 0f);
        return collider.Where(x => x.gameObject.CompareTag("Ground")).ToList().Count != 0;
    }

    // Check if the player is near a wall (either left or right)
    int WallCheck()
    {
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(wallCheckLeft.position, Vector2.left, 0.15f);
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(wallCheckRight.position, Vector2.right, 0.15f);

        bool isWallOnLeft = hitsLeft.Where(x => x.collider.gameObject.CompareTag("Ground")).ToList().Count != 0;
        bool isWallOnRight = hitsRight.Where(x => x.collider.gameObject.CompareTag("Ground")).ToList().Count != 0;

        if (isWallOnLeft) return -1; // Wall on the left
        if (isWallOnRight) return 1; // Wall on the right

        return 0; // No wall detected
    }
}
