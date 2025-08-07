using UnityEngine;

public class JumpingState : AirState
{
    public JumpingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    float jumpTime = 0f;

    public override void StateEnter()
    {
        /*player.JumpInput = false;

        player.RB.linearVelocityY = 0f;
        player.RB.AddForce(new Vector2(0, player.PlayerData.initialJumpHeight), ForceMode2D.Impulse);

        jumpTime = 0f;*/

        //Movement movement = player.Components.GetCoreComponent<Movement>();
        //movement.Jump();

        player.Animator.SetBool("Jumping", true);
    }

    public override void StateUpdate()
    {
        //player.RB.AddForce(new Vector2(0, player.PlayerData.jumpForce), ForceMode2D.Force);

        if (jumpTime > player.PlayerData.minJumpTime && !player.JumpButtonPressed)
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }

        if(jumpTime > player.PlayerData.maxJumpTime)
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }

        jumpTime += Time.deltaTime;
    }
}
