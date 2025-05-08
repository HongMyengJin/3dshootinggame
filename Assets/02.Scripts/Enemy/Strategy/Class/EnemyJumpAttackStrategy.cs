using Unity.VisualScripting;
using UnityEngine;

public class EnemyJumpAttackStrategy : EnemyAttackStrategyBase
{
    public EnemyJumpAttackStrategy()
    {
        _cooldown = 5f;
        _duration = 1.5f;
    }

    protected override void StartAttack(IEnemyAttackContext ctx)
    {
        ctx.Animator.SetTrigger("JumpAttack");
    }
}
