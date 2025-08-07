using UnityEngine;

public class Movement : ComponentBase
{
    public PlayerData PlayerData;
    public Transform PlayerTransform;

    BoxCollider2D boxCollider;

    protected override void Awake()
    {
        base.Awake();
        boxCollider = PlayerTransform.GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        if (player.IsGrounded) player.Velocity.y = 0;

        player.Velocity.y += Physics2D.gravity.y * Time.deltaTime;

        PlayerTransform.Translate(player.Velocity * Time.deltaTime);

        player.IsGrounded = false;
        //TODO: make new file called "collisions" and move this there

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(PlayerTransform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                PlayerTransform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && player.Velocity.y < 0)
                {
                    player.IsGrounded = true;
                }
            }
        }
    }

    public void Move()
    { 
        float acceleration = player.IsGrounded ? player.PlayerData.groundAcceleration : player.PlayerData.airAcceleration;
        float deceleration = player.IsGrounded ? player.PlayerData.groundDeceleration : player.PlayerData.airDeceleration;

        if (player.Movement.x != 0)
        {
            player.Velocity.x = Mathf.MoveTowards(player.Velocity.x, player.PlayerData.movementSpeed * player.Movement.x, acceleration * Time.deltaTime);
        }
        else
        {
            player.Velocity.x = Mathf.MoveTowards(player.Velocity.x, 0, deceleration * Time.deltaTime);
        }
    }
}
