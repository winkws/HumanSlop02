using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerData PlayerData;

    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }
    public Animator Animator { get; private set; }

    public bool IsGrounded { get; set; }
    public bool IsTouchingWall { get; set; }
    public float WallDirection { get; set; }
    public bool DashOnCooldown { get; set; }
    public bool CanDash { get; set; }

    public bool JumpInput { get; set; }
    public bool DashInput { get; set; }

    public bool JumpButtonPressed { get; set; }
    public bool DashButtonPressed { get; set; }

    public Vector2 Movement { get; set; }

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        StateMachine = new PlayerStateMachine(this);
        StateMachine.Initialize();
        StateMachine.ChangeState(StateMachine.IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.StateUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.StateFixedUpdate();
    }
}
