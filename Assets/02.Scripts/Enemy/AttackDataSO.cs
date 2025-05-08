using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackDataSO")]
public class AttackDataSO : ScriptableObject
{
    [Header("공격 설정")]
    [Tooltip("공격 범위")]
    public float attackRange;

    [Tooltip("공격 각도 (부채꼴 범위 각도)")]
    [Range(0, 180)]
    public float attackAngle;

    [Tooltip("공격 데미지")]
    public int attackDamage;

    [Header("휘두르기 설정")]
    [Tooltip("휘두르기 동안 공격 판정 시간")]
    public float swingDuration;

    [Tooltip("휘두르는 동안 판정 간격")]
    public float hitInterval;
}