using UnityEngine;

public interface IPlayerDamageContext : IPlayerContext
{
    Animator Animator { get; }
    Vector3 KnockbackDirection { get; }

}
