using UnityEngine;

public class EnemyShieldDefenseStrategy : EnemyAttackStrategyBase
{
    public EnemyShieldDefenseStrategy()
    {
        _cooldown = 6f;
        _duration = 1f;
    }

    protected override void StartAttack(IEnemyAttackContext ctx)
    {
        ctx.Animator.SetBool("Shield", true);
    }

    public override void Exit(IEnemyAttackContext ctx)
    {
        ctx.Animator.SetBool("Shield", false);
    }

}
