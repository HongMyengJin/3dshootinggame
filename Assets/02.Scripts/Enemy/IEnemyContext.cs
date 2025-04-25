using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public interface IEnemyContext
{
    Transform Self { get; } // 본인 위치
    Transform Target { get; } // 타겟 위치
    NavMeshAgent Agent { get; }
    CharacterController Controller { get; }

    EnemyStatSO State { get;  }
    Vector3 StartPoint { get; }
    Vector3 KnockbackDirection { get; }
    Coroutine StartCoroutine(IEnumerator routine);
    void StopCoroutine(Coroutine coroutine);
    void ScheduleStateChange(EnemyStateType next, float delay = 0.0f);
    void SetDestination(Vector3 targetPosition);
}
