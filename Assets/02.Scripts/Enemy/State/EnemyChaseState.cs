using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx)
    {
        context = ctx;
    }
    public void Update()
    {
        if (Vector3.Distance(context.Self.position, context.Target.position) >= context.State.ReturnDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Return);
            return;
        }

        if (Vector3.Distance(context.Self.position, context.Target.position) < context.State.AttackDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Attack);
            return;
        }

        context.SetDestination(context.Target.position);
    }
    public void Exit()
    {

    }
}
