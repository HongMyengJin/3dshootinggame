using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public interface IEnemyDamagedContext : IEnemyContext
{
    Vector3 KnockbackDirection { get; }
}
