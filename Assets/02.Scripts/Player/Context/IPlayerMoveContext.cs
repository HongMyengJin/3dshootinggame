using UnityEngine;

public interface IPlayerMoveContext : IPlayerContext
{
    Vector2 Input { get; }
    float Speed { get; }
    Animator Animator { get; }
    CharacterController CharacterController { get; }
}
