using UnityEngine;

public class WalkingState : GroundedState
{
    public WalkingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void StateUpdate()
    {
        if (player.Movement.x == 0)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        player.Animator.SetInteger("Movement", (int)player.Movement.x);
    }
}
