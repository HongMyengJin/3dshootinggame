using UnityEngine;

public class EnemyReturnState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx) => context = ctx;
    public void Update()
    {
        if (Vector3.Distance(context.Self.position, context.StartPoint) <= context.State.DistanceGap)
        {
            context.Self.position = context.StartPoint;
            context.ScheduleStateChange(EnemyStateType.Idle);
        }

        if (Vector3.Distance(context.Self.position, context.Target.position) < context.State.FindDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Chase);
        }
        context.Agent.SetDestination(context.Target.position);
    }
    public void Exit()
    {

    }
}
