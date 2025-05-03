using UnityEngine;

public interface IPlayerMoveContext : IPlayerContext
{
    Transform Transform { get; }
    Vector2 Input { get; }
    float Speed { get; }
    Animator Animator { get; }
    CharacterController CharacterController { get; }
}
