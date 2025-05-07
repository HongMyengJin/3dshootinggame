using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttackType
{
    Punch,
    Jump,
    Throw,
    Shield,
    EnemyAttackTypeEnd
}

public abstract class EnemyAttackStateBase : IEnemyState
{
    protected IEnemyAttackContext context;
    protected Dictionary<EnemyAttackType, EnemyAttackStrategyBase> strategies;

    public virtual void Enter(IEnemyContext ctx) { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
