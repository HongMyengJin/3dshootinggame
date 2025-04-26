using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public interface IEnemyPatrolContext : IEnemyContext
{
    Transform[] PatrolPoints { get; }
    int PatrolIndex { get; }
    void MoveToNextPatrolPoint();
}
