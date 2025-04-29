using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using TMPro;

public class EnemyAttackStragegy : IEnemyStrategy<IEnemyAttackContext>
{
    private Coroutine _attackRoutine;
    private readonly float _attackDelay = 3.0f;

    public void Execute(IEnemyAttackContext context)
    {
        if (context == null)
            return;
        _attackRoutine = context.StartCoroutine(AttackLoop(context.Animator));
    }
    public void Update(IEnemyAttackContext context)
    {
        Vector3 targetDirection = (context.Target.position - context.Self.position).normalized;
        context.Self.rotation = Quaternion.LookRotation(targetDirection);
    }
    private IEnumerator AttackLoop(Animator animator)
    {
        while (true)
        {
            animator.SetTrigger("Shoot");
            yield return new WaitForSeconds(_attackDelay);
        }
    }

    public void Exit(IEnemyAttackContext context)
    {
        context.StopCoroutine(_attackRoutine);
    }
}
