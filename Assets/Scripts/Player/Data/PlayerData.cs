using UnityEngine;

[CreateAssetMenu(fileName = "playerData")]
public class PlayerData : ScriptableObject
{
    [Header("Basic Movement")]
    public float movementSpeed;

    [Header("Jump")]
    public float initialJumpHeight; 
    public float jumpForce;
    public float minJumpTime;
    public float maxJumpTime;
    public float landingTime;

    [Header("Dash")]
    public float dashSpeed;
    public float dashLength;
    public float dashCooldown;

    [Header("Wall Slide")]
    [Range(0.0f, 1.0f)]
    public float wallSlideSpeed;

    [Header("Wall Jump")]
    public float wallJumpInitialHorizontalDistance;
    public float wallJumpInitialVerticalDistance;
    public float wallJumpHorizontalForce;
    public float wallJumpVerticalForce;
    public float minWallJumpTime;
    public float maxWallJumpTime;

    [Header("Air Movement")]
    [Range(0.0f, 1.0f)]
    public float airMovementPenalty;

    [Header("Advanced")]
    public float coyoteTime;

    [Header("Buffers")]
    public float jumpInputBuffer;
    public float dashInputBuffer;
}