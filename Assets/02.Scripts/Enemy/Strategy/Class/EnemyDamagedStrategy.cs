using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyDamagedStragegy : IEnemyStrategy<IEnemyDamagedContext>
{
    public void Execute(IEnemyDamagedContext context)
    {
        context.StartCoroutine(Knockback(context));
        context.StartCoroutine(Stun(context));
    }
    private IEnumerator Knockback(IEnemyDamagedContext context)
    {
        float elapsed = 0;
        float duration = context.State.KnockbackDuration;
        Vector3 direction = context.KnockbackDirection;

        while (elapsed < duration)
        {
            float value = Mathf.Lerp(0, context.State.KnockbackMaxSpeed, elapsed / duration);
            context.Controller.Move(direction * value * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator Stun(IEnemyDamagedContext context)
    {
        // 코루틴 방식으로 변경
        context.Agent.isStopped = true;
        context.Agent.ResetPath();
        yield return new WaitForSeconds(context.State.DamagedStunTime);
        context.ScheduleStateChange(EnemyStateType.Chase);
    }
}
