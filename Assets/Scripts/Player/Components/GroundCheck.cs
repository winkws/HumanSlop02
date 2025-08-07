using UnityEngine;

public class GroundCheck : ComponentBase
{
    private readonly RaycastHit2D[] _groundCheckHits = new RaycastHit2D[1];

    public Transform groundCheckLeft;
    public Transform groundCheckMiddle;
    public Transform groundCheckRight;

    private void Start()
    {
        IsActive = true;
    }

    public override void ComponentUpdate()
    {
        //player.IsGrounded = Check();
    }

    private bool Check()
    {
        int hitCountLeft = Physics2D.RaycastNonAlloc(groundCheckLeft.position, Vector2.down, _groundCheckHits, 0.025f, LayerMask.GetMask("Ground"));
        if (hitCountLeft > 0 && _groundCheckHits[0].collider != null) { return true; }

        int hitCountMiddle = Physics2D.RaycastNonAlloc(groundCheckMiddle.position, Vector2.down, _groundCheckHits, 0.025f, LayerMask.GetMask("Ground"));
        if (hitCountMiddle > 0 && _groundCheckHits[0].collider != null) { return true; }

        int hitCountRight = Physics2D.RaycastNonAlloc(groundCheckRight.position, Vector2.down, _groundCheckHits, 0.025f, LayerMask.GetMask("Ground"));
        if (hitCountRight > 0 && _groundCheckHits[0].collider != null) { return true; }

        return false;
    }
}
