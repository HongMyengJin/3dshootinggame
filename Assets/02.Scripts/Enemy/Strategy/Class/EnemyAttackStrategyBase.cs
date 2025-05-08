using UnityEngine;
using System.Collections;

public abstract class EnemyAttackStrategyBase : IEnemyStrategy<IEnemyAttackContext>
{
    protected float _cooldown;
    protected float _duration;
    protected float _lastUsedTime;
    protected bool _isFinished;

    public bool CanUse() => Time.time - _lastUsedTime > _cooldown;
    public bool IsFinished() => _isFinished;

    public virtual void Execute(IEnemyAttackContext ctx)
    {
        _isFinished = false;
        _lastUsedTime = Time.time;
        StartAttack(ctx);
        ctx.StartCoroutine(EndAfter(ctx));
    }

    protected abstract void StartAttack(IEnemyAttackContext ctx);

    protected virtual IEnumerator EndAfter(IEnemyAttackContext ctx)
    {
        yield return new WaitForSeconds(_duration);
        _isFinished = true;
    }

    public virtual void Update(IEnemyAttackContext ctx) { }
    public virtual void Exit(IEnemyAttackContext ctx) { }
}