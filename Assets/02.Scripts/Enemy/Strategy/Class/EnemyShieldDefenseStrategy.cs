using UnityEngine;

public class EnemyShieldDefenseStrategy : EnemyAttackStrategyBase
{
    private ShieldController _shieldController;
    public EnemyShieldDefenseStrategy(ShieldController shieldController)
    {
        _shieldController = shieldController;

        _cooldown = 6f;
        _duration = 1f;
    }

    protected override void StartAttack(IEnemyAttackContext ctx)
    {
        ctx.Animator.SetBool("Shield", true);
        _shieldController.Show();
    }

    public override void Exit(IEnemyAttackContext ctx)
    {
        ctx.Animator.SetBool("Shield", false);
        _shieldController.Hide();
    }

}
