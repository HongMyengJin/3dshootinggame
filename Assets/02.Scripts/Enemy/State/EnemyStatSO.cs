using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatSO", menuName = "Scriptable Objects/EnemyStatSO")]
public class EnemyStatSO : ScriptableObject
{
    [Header("AI 감지 및 범위")]
    [SerializeField] private float findDistance; //  = 5.0f;        // 플레이어 발견 범위
    [SerializeField] private float attackDistance; // = 2.5f;      // 플레이어 공격 범위
    [SerializeField] private float returnDistance; // = 10.0f;     // 복귀 범위
    [SerializeField] private float distanceGap; // = 0.1f;

    [Header("이동 및 전투")]
    [SerializeField] private float moveSpeed; // = 3.3f;
    [SerializeField] private float attackCooldown; // = 2.0f;

    [Header("스탯")]
    [SerializeField] private int maxHealth; // = 100;

    [Header("리액션/연출 관련")]
    [SerializeField] private float damagedStunTime; // = 1.0f;     // 피격 시 경직
    [SerializeField] private float deathDelay; // = 0.2f;          // 죽음 이후 시간
    [SerializeField] private float patrolCheckTime; // = 3.0f;     // Idle 상태에서 행동 결정 시간

    [Header("넉백 설정")]
    [SerializeField] private float knockbackDuration; // = 0.5f;
    [SerializeField] private float knockbackMaxSpeed; // = 20.0f;

    // 읽기 전용 프로퍼티
    public float FindDistance => findDistance;
    public float AttackDistance => attackDistance;
    public float ReturnDistance => returnDistance;

    public float MoveSpeed => moveSpeed;
    public float AttackCooldown => attackCooldown;

    public int MaxHealth => maxHealth;
    public float DamagedStunTime => damagedStunTime;
    public float DeathDelay => deathDelay;
    public float PatrolWaitTime => patrolCheckTime;

    public float KnockbackDuration => knockbackDuration;
    public float KnockbackMaxSpeed => knockbackMaxSpeed;

    public float DistanceGap => distanceGap;
}
