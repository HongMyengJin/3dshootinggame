using UnityEngine;

public enum EnemyStateType
{
    Idle, // 대기
    Patrol, // 순찰
    Chase, // 추적
    Follow, // 따라가기 -> 다른 상태 변환x
    Return, // 복귀
    Attack, // 공격
    Damaged, // 피격
    Die // 사망
}
