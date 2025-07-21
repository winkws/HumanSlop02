public abstract class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;

    public SuperState SuperState;

    protected PlayerState(Player player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void InitializeState() { }
    public virtual void StateEnter() { }
    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateExit() { }
}