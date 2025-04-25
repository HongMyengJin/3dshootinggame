using UnityEngine;

public enum EnemyStateType
{
    Idle, // 대기
    Patrol, // 순찰
    Trace, // 추적
    Return, // 복귀
    Attack, // 공격
    Damaged, // 피격
    Die // 사망
}