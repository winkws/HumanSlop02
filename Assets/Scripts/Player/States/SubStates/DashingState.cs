using UnityEngine;

public class DashingState : AbilityState
{
    public DashingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    float defaultGravity;
    float timer = 0f;

    public override void StateEnter()
    {
        timer = 0f;

        //defaultGravity = player.RB.gravityScale;
        player.DashInput = false;
        player.CanDash = false;

        //player.RB.linearVelocity = Vector2.zero;
        //player.RB.gravityScale = 0f;
        //player.RB.AddForce(player.Movement.normalized * player.PlayerData.dashSpeed, ForceMode2D.Impulse);

        player.Animator.SetBool("Dashing", true);
    }

    public override void StateUpdate()
    {
        timer += Time.deltaTime;

        if (timer < player.PlayerData.dashLength) return;

        if (player.IsGrounded)
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
        else
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }
    }

    public override void StateExit()
    {
        //player.RB.gravityScale = defaultGravity;
        //player.RB.linearVelocity = Vector2.zero;

        player.GetComponent<Timers>().DashCoroutine();

        player.Animator.SetBool("Dashing", false);
    }
}
