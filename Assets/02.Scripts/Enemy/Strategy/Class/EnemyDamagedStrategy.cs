using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyDamagedStragegy : IEnemyStrategy<IEnemyDamagedContext>
{
    public void Execute(IEnemyDamagedContext context)
    {
        if (context == null)
            return;
        context.StartCoroutine(Knockback(context));
    }
    public void Update(IEnemyDamagedContext context)
    {

    }
    public void Exit(IEnemyDamagedContext context)
    {

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

}
