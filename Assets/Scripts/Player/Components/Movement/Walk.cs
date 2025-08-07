using UnityEngine;

public class Walk
{
    public static Player player;

    static public Vector2 Move(Vector2 velocity)
    {
        /*float acceleration = player.IsGrounded ? player.PlayerData.walkAcceleration : player.PlayerData.airAcceleration;
        float deceleration = player.IsGrounded ? player.PlayerData.groundDeceleration : 0;

        if (player.Movement.x != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, player.PlayerData.movementSpeed * player.Movement.x, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }
        
        return velocity;*/

        return Vector2.zero;

        //if (player.IsGrounded) velocity.y = 0;

        //velocity.y += Physics2D.gravity.y * Time.deltaTime;



        //Debug.Log(velocity);

        /*player.IsGrounded = false;
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
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    player.IsGrounded = true;
                }
            }
        }*/
    }
}
