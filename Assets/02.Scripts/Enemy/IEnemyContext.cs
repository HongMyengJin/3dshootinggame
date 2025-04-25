using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public interface IEnemyContext
{
    Transform Self { get; } // 본인 위치
    Transform Target { get; } // 타겟 위치
    NavMeshAgent Agent { get; }
    CharacterController Controller { get; }


    Vector3 StartPoint { get; }
    EnemyStatSO State { get;  }

    Vector3 KnockbackDirection { get; }
    Transform[] PatrolPoints { get; }
    int PatrolIndex { get; }
    Coroutine StartCoroutine(IEnumerator routine);
    void StopCoroutine(Coroutine coroutine);
    void ScheduleStateChange(EnemyStateType next, float delay = 0.0f);
    void MoveToNextPatrolPoint();
    void SetDestination(Vector3 targetPosition);
}
