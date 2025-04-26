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
                EnemyStateType.Idle => new EnemyIdleState(new EnemyIdleStrategy()),
                EnemyStateType.Patrol => new EnemyPatrolState(new EnemyPatrolStragegy()),
                EnemyStateType.Chase => new EnemyChaseState(new EnemyChaseStragegy()),
                EnemyStateType.Return => new EnemyReturnState(new EnemyReturnStragegy()),
                EnemyStateType.Attack => new EnemyAttackState(new EnemyAttackStragegy()),
                EnemyStateType.Damaged => new EnemyDamagedState(new EnemyDamagedStragegy()),
                EnemyStateType.Die => new EnemyDieState(new EnemyDieStragegy()),
                _ => null // 그 외
            };
            stateCache[type] = state;
        }
        return state;
    }
}
