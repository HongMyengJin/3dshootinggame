using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseStragegy : IEnemyStrategy<IEnemyChaseContext>
{
    public void Execute(IEnemyChaseContext context)
    {
        if (context == null)
            return;

        Vector3 dir = (context.Target.transform.position - context.Self.transform.position).normalized;
        context.Controller.Move(dir * context.State.MoveSpeed * Time.deltaTime);
    }
}
