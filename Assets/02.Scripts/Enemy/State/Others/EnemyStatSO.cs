using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatSO", menuName = "Scriptable Objects/EnemyStatSO")]
public class EnemyStatSO : ScriptableObject
{
    [Header("AI 감지 및 범위")]
    [SerializeField] private float findDistance; 
    [SerializeField] private float attackDistance; 
    [SerializeField] private float returnDistance; 
    [SerializeField] private float distanceGap;

    [Header("이동 및 전투")]
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float attackCooldown; 

    [Header("스탯")]
    [SerializeField] private int maxHealth;

    [Header("리액션/연출 관련")]
    [SerializeField] private float damagedStunTime; 
    [SerializeField] private float deathDelay; 
    [SerializeField] private float patrolCheckTime; 

    [Header("넉백 설정")]
    [SerializeField] private float knockbackDuration; 
    [SerializeField] private float knockbackMaxSpeed; 

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
