using UnityEngine;
using System.Collections;

public interface IEnemyContext
{
    Transform Target { get; } // 타겟 위치

    EnemyStatSO State { get;  }

    Coroutine StartCoroutine(IEnumerator routine);
    void StopCoroutine(Coroutine coroutine);
    void ScheduleStateChange(EnemyStateType next, float delay);
}
