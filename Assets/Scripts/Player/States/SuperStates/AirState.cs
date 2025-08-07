using UnityEngine;

public class AirState : PlayerState
{
    public AirState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) 
    {
        SuperState = SuperState.Air;
    }

    public override void StateFixedUpdate()
    {
        Movement movement = player.Components.GetCoreComponent<Movement>();
        movement.Move();

        //float playerMovement = player.Movement.x * player.PlayerData.movementSpeed * player.PlayerData.airMovementPenalty * Time.deltaTime;
        //player.transform.Translate(new Vector2(playerMovement, 0));

        if (player.Movement.x != 0)
        {
            player.SR.flipX = player.Movement.x < 0;
        }

        if (player.DashInput && !player.DashOnCooldown && player.CanDash && player.Movement != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.DashingState);
            return;
        }

        if (player.IsTouchingWall && player.Movement.x == player.WallDirection && stateMachine.CurrentState != stateMachine.WallSlidingState)
        {
            stateMachine.ChangeState(stateMachine.WallSlidingState);
        }
    }
}
