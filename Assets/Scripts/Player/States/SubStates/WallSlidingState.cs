using UnityEngine;

public class WallSlidingState : AirState
{
    public WallSlidingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    float defaultGravity;

    public override void StateEnter()
    {
        player.RB.linearVelocityY = 0f;

        defaultGravity = player.RB.gravityScale;
        player.RB.gravityScale = player.PlayerData.wallSlideSpeed;

        player.Animator.SetBool("WallHugging", true);
    }

    public override void StateUpdate()
    {
        if (player.JumpInput)
        {
            stateMachine.ChangeState(stateMachine.WallJumpingState);
            return;
        }

        if (player.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.LandingState);
            return;
        }

        if (!player.IsTouchingWall && !player.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.FallingState);
            return;
        }
    }

    public override void StateExit()
    {
        player.RB.gravityScale = defaultGravity;

        player.Animator.SetBool("WallHugging", false);
    }
}
