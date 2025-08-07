using UnityEngine;

public class IdleState : GroundedState
{
    public IdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void StateEnter()
    {
        //player.RB.linearVelocityX = 0f;
        player.Animator.SetInteger("Movement", 0);
    }

    public override void StateUpdate()
    {
        if(player.Movement.x != 0)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);
        }
    }
}
