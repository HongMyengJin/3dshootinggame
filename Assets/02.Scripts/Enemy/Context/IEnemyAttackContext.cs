using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public interface IEnemyAttackContext : IEnemyContext
{
    Collider Collider { get; }
    bool ShouldBlock();
}
