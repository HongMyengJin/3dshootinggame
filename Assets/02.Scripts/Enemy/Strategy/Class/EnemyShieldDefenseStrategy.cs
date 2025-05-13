using System.Linq;
using UnityEngine;

public class EnemyShieldDefenseStrategy : EnemyAttackStrategyBase
{
    private DissolveController _shieldController;
    public EnemyShieldDefenseStrategy(DissolveController shieldController)
    {
        _shieldController = shieldController;

        _cooldown = 5.0f;
        _duration = 5f;
    }

    protected override void StartAttack(IEnemyAttackContext ctx)
    {
        AnimationClip clip = ctx.Animator.runtimeAnimatorController.animationClips
        .FirstOrDefault(c => c.name == "Shield");
        _duration = clip.length;

        ctx.Animator.SetBool("Shield", true);
        _shieldController.Show();
    }

    public override void Exit(IEnemyAttackContext ctx)
    {
        ctx.Animator.SetBool("Shield", false);
        _shieldController.Hide();
    }

    public override void Update(IEnemyAttackContext ctx)
    {
        Transform transform = ctx.Self;
        Transform targetTransform = ctx.Target;

        Vector3 direction = (targetTransform.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);
    }

}
