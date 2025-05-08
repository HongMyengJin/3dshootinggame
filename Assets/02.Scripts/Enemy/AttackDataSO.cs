using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackDataSO")]
public class AttackDataSO : ScriptableObject
{
    [Header("���� ����")]
    [Tooltip("���� ����")]
    public float attackRange;

    [Tooltip("���� ���� (��ä�� ���� ����)")]
    [Range(0, 180)]
    public float attackAngle;

    [Tooltip("���� ������")]
    public int attackDamage;

    [Header("�ֵθ��� ����")]
    [Tooltip("�ֵθ��� ���� ���� ���� �ð�")]
    public float swingDuration;

    [Tooltip("�ֵθ��� ���� ���� ����")]
    public float hitInterval;
}