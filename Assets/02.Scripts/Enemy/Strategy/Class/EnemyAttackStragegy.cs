using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using TMPro;

public class EnemyAttackStragegy : IEnemyStrategy<IEnemyAttackContext>
{
    private Coroutine _attackRoutine;
    private readonly float _attackDelay = 5.0f;

    public void Execute(IEnemyAttackContext context)
    {
        if (context == null)
            return;
        _attackRoutine = context.StartCoroutine(AttackLoop(context.Animator));
    }
    public void Update(IEnemyAttackContext context)
    {
        float speed = context.Agent.velocity.magnitude;
        context.Animator.SetFloat("MoveSpeed", speed);

        Vector3 targetDirection = (context.Target.position - context.Self.position).normalized;

        targetDirection.y = 0.0f;
        context.Self.rotation = Quaternion.LookRotation(targetDirection);
    }
    private IEnumerator AttackLoop(Animator animator)
    {
        while (true)
        {
            int rand = Random.Range(0, 4); // 0 ~ 3
            animator.SetInteger("AttackType", rand);
            animator.SetTrigger("Attack");

            yield return new WaitForSeconds(_attackDelay);
        }
    }

    public void Exit(IEnemyAttackContext context)
    {
        context.Animator.SetBool("Attack", false);
        context.StopCoroutine(_attackRoutine);
    }

    public void OnAttackAnimationEvent()
    {
        Debug.Log("총알 나가는 시점!!!");
    }
}
