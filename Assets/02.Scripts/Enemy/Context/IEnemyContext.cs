using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public interface IEnemyContext
{
    Transform Self { get; } // 본인 위치
    Transform Target { get; } // 타겟 위치
    // CharacterController Controller { get; }
    NavMeshAgent Agent { get; }
    EnemyStatSO State { get;  }

    Animator Animator { get; }

    Coroutine StartCoroutine(IEnumerator routine);
    void StopCoroutine(Coroutine coroutine);
    void ScheduleStateChange(EnemyStateType next, float delay = 0.0f);

    void SetDestination(Vector3 targetPosition);
}
