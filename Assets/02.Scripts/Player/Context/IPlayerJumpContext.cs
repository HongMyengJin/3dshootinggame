using UnityEngine;

public interface IPlayerJumpContext : IPlayerContext
{
    bool IsGrounded { get; }
    float JumpForce { get; }
    CharacterController CharacterController { get; }
}
