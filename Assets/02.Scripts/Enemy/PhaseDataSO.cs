using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhaseDataSO", menuName = "Scriptable Objects/PhaseDataSO")]
public class PhaseDataSO : ScriptableObject
{
    [Range(0f, 1f)] public float minHpPercent;
    [Range(0f, 1f)] public float maxHpPercent;
    public List<EnemyAttackType> allowedAttacks;
}