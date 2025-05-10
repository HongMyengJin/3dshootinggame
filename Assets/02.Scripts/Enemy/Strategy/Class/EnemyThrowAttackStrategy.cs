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

    public override void Update(IEnemyAttackContext ctx)
    {
        Transform transform = ctx.Self;
        Transform targetTransform = ctx.Target;

        Vector3 direction = (targetTransform.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion rotationOffset = Quaternion.Euler(0, 0.0f, 0);
        targetRotation *= rotationOffset;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);

    }
}
