using UnityEngine;

public class LandingState : GroundedState
{
    public LandingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    float timer = 0f;

    public override void StateEnter()
    {
        player.Animator.SetBool("Jumping", false);
        player.Animator.SetInteger("Movement", 0);
        //player.RB.linearVelocityX = 0f;
        player.CanDash = true;
        timer = 0f;
    }

    public override void StateFixedUpdate() { }

    public override void StateUpdate()
    {
        if(timer >= player.PlayerData.landingTime)
        {
            if(player.Movement.x == 0)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.WalkingState);
            }
        }

        timer += Time.deltaTime;
    }
}
