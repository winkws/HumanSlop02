using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    float jumpInputBuffer;
    float dashInputBuffer;
    float jumpTimer;
    float dashTimer;

    Player player;

    InputAction movementAction;
    InputAction jumpAction;
    InputAction dashAction;

    private void Start()
    {
        player = GetComponent<Player>();

        jumpInputBuffer = player.PlayerData.jumpInputBuffer;
        dashInputBuffer = player.PlayerData.dashInputBuffer;
    }

    private void Awake()
    {
        movementAction = InputSystem.actions.FindAction("Movement");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
    }

    private void Update()
    {
        player.Movement = movementAction.ReadValue<Vector2>();
        player.JumpButtonPressed = jumpAction.IsPressed();
        player.DashButtonPressed = dashAction.IsPressed();

        if (jumpAction.WasPressedThisFrame())
        {
            player.JumpInput = true;
            jumpTimer = 0f;
        }
        else if (player.JumpInput && jumpTimer > jumpInputBuffer)
        {
            player.JumpInput = false;
        }

        if (dashAction.WasPressedThisFrame())
        {
            player.DashInput = true;
            dashTimer = 0f;
        }
        else if(player.DashInput && dashTimer > dashInputBuffer)
        {
            player.DashInput = false;
        }

        jumpTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
    }
}
