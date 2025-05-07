using UnityEngine;

public class EnemyThrowAttackStrategy : EnemyAttackStrategyBase
{
    public EnemyThrowAttackStrategy()
    {
        _cooldown = 4f;
        _duration = 1.2f;
    }
    protected override void StartAttack(IEnemyAttackContext ctx)
    {
        ctx.Animator.SetTrigger("Throw");
    }
}
