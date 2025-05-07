using UnityEngine;

public class EnemyPunchAttackStrategy : EnemyAttackStrategyBase
{
    public EnemyPunchAttackStrategy()
    {
        _cooldown = 4f;
        _duration = 1.2f;
    }
    protected override void StartAttack(IEnemyAttackContext ctx)
    {
        ctx.Animator.SetTrigger("Punch");
    }
}
