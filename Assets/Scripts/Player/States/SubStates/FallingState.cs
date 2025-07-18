using UnityEngine;

public class FallingState : AirState
{
    public FallingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void StateEnter()
    {
        player.Animator.SetBool("Jumping", true);
    }

    public override void StateUpdate()
    {
        if (player.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.LandingState);
        }
    }
}
