public enum SuperState
{
    Ground,
    Air,
    Ability,
}

public class PlayerStateMachine
{
    public Player player;

    public PlayerState CurrentState { get; private set; }

    public IdleState IdleState { get; private set; }
    public WalkingState WalkingState { get; private set; }
    public JumpingState JumpingState { get; private set; }
    public FallingState FallingState { get; private set; }
    public LandingState LandingState { get; private set; }
    public WallSlidingState WallSlidingState { get; private set; }
    public WallJumpingState WallJumpingState { get; private set; }
    public DashingState DashingState { get; private set; }

    public PlayerStateMachine(Player player)
    {
        this.player = player;
    }

    public void Initialize()
    {
        IdleState = new IdleState(player, this);
        WalkingState = new WalkingState(player, this);
        JumpingState = new JumpingState(player, this);
        FallingState = new FallingState(player, this);
        LandingState = new LandingState(player, this);
        WallSlidingState = new WallSlidingState(player, this);
        WallJumpingState = new WallJumpingState(player, this);
        DashingState = new DashingState(player, this);

        IdleState.InitializeState();
        WalkingState.InitializeState();
        JumpingState.InitializeState();
        FallingState.InitializeState();
        LandingState.InitializeState();
        WallSlidingState.InitializeState();
        WallJumpingState.InitializeState();
        DashingState.InitializeState();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState?.StateExit();
        CurrentState = newState;
        CurrentState?.StateEnter();
    }
}