using UnityEngine;

public class Collisions : ComponentBase
{
    public Transform PlayerTransform;

    BoxCollider2D boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = PlayerTransform.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && player.Velocity.y < 0)
                {
                    player.IsGrounded = true;
                }
            }
        }*/
    }
}
