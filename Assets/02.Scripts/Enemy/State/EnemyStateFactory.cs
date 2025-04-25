using System.Collections.Generic;
using UnityEngine;

public static class EnemyStateFactory
{
    private static readonly Dictionary<EnemyStateType, IEnemyState> stateCache = new();

    public static IEnemyState Get(EnemyStateType type)
    {
        if(!stateCache.TryGetValue(type, out var state)) // 없으면 생성
        {
            state = type switch
            {
                EnemyStateType.Idle => new EnemyIdleState(),
                EnemyStateType.Patrol => new EnemyPatrolState(),
                EnemyStateType.Chase => new EnemyChaseState(),
                EnemyStateType.Return => new EnemyReturnState(),
                EnemyStateType.Attack => new EnemyAttackState(),
                EnemyStateType.Damaged => new EnemyDamagedState(),
                EnemyStateType.Die => new EnemyDieState(),
                _ => null // 그 외
            };
            stateCache[type] = state;
        }
        return state;
    }
}
