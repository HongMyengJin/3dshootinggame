using System.Linq;
using UnityEngine;

public class EnemyThrowAttackStrategy : EnemyAttackStrategyBase
{
    public EnemyThrowAttackStrategy()
    {
        _cooldown = 2.0f;
        _duration = 7.0f;
    }
    protected override void StartAttack(IEnemyAttackContext ctx)
    {
        AnimationClip clip = ctx.Animator.runtimeAnimatorController.animationClips
        .FirstOrDefault(c => c.name == "Casting Spell");
        _duration = clip.length;
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
