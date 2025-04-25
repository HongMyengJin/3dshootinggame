using UnityEngine;

public interface IEnemyContext
{
    Transform Self { get; } // 내 자신 위치
    Transform Target { get; } // 타겟 위치
    float AttackDistance { get; } // 공격 거리
    float Speed { get; } // 속도

    void ChangeState(EnemyStateType nextState);
    void TriggerAttack();
}
