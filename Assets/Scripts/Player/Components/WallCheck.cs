using UnityEngine;

public class WallCheck : ComponentBase
{
    private readonly RaycastHit2D[] _wallCheckHits = new RaycastHit2D[1];

    Transform wallCheckLeft;
    Transform wallCheckRight;

    private void Start()
    {
        wallCheckLeft = GameObject.Find("WallCheckLeft").transform;
        wallCheckRight = GameObject.Find("WallCheckRight").transform;

        IsActive = true;
    }

    public override void ComponentUpdate()
    {
        int result = Check();

        if (result == 0)
        {
            player.IsTouchingWall = false;
            return;
        }

        player.IsTouchingWall = true;
        player.WallDirection = result;
    }

    int Check()
    {
        // Left check
        int hitCountLeft = Physics2D.RaycastNonAlloc(wallCheckLeft.position, Vector2.left, _wallCheckHits, 0.25f, LayerMask.GetMask("Ground"));
        if (hitCountLeft > 0 && _wallCheckHits[0].collider != null)
        {
            return -1; // Wall on the left
        }

        // Right check
        int hitCountRight = Physics2D.RaycastNonAlloc(wallCheckRight.position, Vector2.right, _wallCheckHits, 0.25f, LayerMask.GetMask("Ground"));
        if (hitCountRight > 0 && _wallCheckHits[0].collider != null)
        {
            return 1; // Wall on the right
        }

        return 0; // No wall detected
    }
}
