using UnityEngine;

public class GroundedState : PlayerState
{
    float coyoteTimer;

    public GroundedState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        SuperState = SuperState.Ground;
    }

    public override void StateFixedUpdate()
    {
        float playerMovement = player.Movement.x * player.PlayerData.movementSpeed;
        player.RB.linearVelocityX = playerMovement;

        if (player.Movement.x != 0)
        {
            player.SR.flipX = player.Movement.x < 0;
        }

        if (player.JumpInput)
        {
            stateMachine.ChangeState(stateMachine.JumpingState);
            return;
        }

        if (player.DashInput && !player.DashOnCooldown && player.Movement != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.DashingState);
            return;
        }

        if (!player.IsGrounded)
        {
            if (coyoteTimer < player.PlayerData.coyoteTime)
            {
                coyoteTimer += Time.deltaTime;
                return;
            }

            stateMachine.ChangeState(stateMachine.FallingState);
            return;
        }

        if (player.IsGrounded)
        {
            coyoteTimer = 0f;
        }
    }
}
