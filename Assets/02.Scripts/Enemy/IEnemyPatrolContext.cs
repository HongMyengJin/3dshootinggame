using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public interface IEnemyPatrolContext
{
    Transform[] PatrolPoints { get; }
    int PatrolIndex { get; }
    void MoveToNextPatrolPoint();
}
