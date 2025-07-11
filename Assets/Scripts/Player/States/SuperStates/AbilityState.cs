using UnityEngine;

public class AbilityState : PlayerState
{
    public AbilityState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) 
    {
        SuperState = SuperState.Ability;
    }
}
