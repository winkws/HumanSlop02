using UnityEngine;

public class WallJumpingState : AirState
{
    public WallJumpingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    float jumpTime = 0f;
    float wallDirection = 0f;

    public override void StateEnter()
    {
        /*player.JumpInput = false;

        jumpTime = 0f;
        wallDirection = player.WallDirection;

        player.Animator.SetBool("Jumping", true);
        player.SR.flipX = player.WallDirection > 0;

        player.RB.linearVelocity = Vector2.zero;
        player.RB.AddForce(new Vector2(player.PlayerData.wallJumpInitialHorizontalDistance * (wallDirection * -1), player.PlayerData.wallJumpInitialVerticalDistance), ForceMode2D.Impulse);*/
    }

    public override void StateFixedUpdate() { }

    public override void StateUpdate()
    {
        /*player.RB.AddForce(new Vector2(player.PlayerData.wallJumpHorizontalForce * (wallDirection * -1), player.PlayerData.wallJumpVerticalForce), ForceMode2D.Force);

        if (jumpTime > player.PlayerData.minWallJumpTime && !player.JumpButtonPressed)
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }

        if (jumpTime > player.PlayerData.maxWallJumpTime)
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }

        jumpTime += Time.deltaTime;*/
    }
}
