using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;
public class EnemyDamagedState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx)
    {
        context = ctx;

        context.StartCoroutine(Knockback(context.State.KnockbackDuration, context.KnockbackDirection));
        context.StartCoroutine(Damaged_Coroutine());
    }
    public void Update()
    {
    }
    public void Exit()
    {

    }

    private IEnumerator Damaged_Coroutine()
    {
        // 코루틴 방식으로 변경
        context.Agent.isStopped = true;
        context.Agent.ResetPath();
        yield return new WaitForSeconds(context.State.DamagedStunTime);
        context.ScheduleStateChange(EnemyStateType.Chase);
    }

    private IEnumerator Knockback(float time, Vector3 dir)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            float value = Mathf.Lerp(0, context.State.KnockbackMaxSpeed, elapsed / time);
            context.Controller.Move(dir * value * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

}
