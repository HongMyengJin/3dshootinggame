using UnityEngine;

public class EnemyReturnState : IEnemyState
{
    private IEnemyReturnContext context;
    private IEnemyStrategy<IEnemyReturnContext> returnStrategy;
    public EnemyReturnState(IEnemyStrategy<IEnemyReturnContext> returnStrategy)
    {
        this.returnStrategy = returnStrategy;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyReturnContext;
    }
    public void Update()
    {
        if (context == null)
            return;

        returnStrategy.Execute(context);

        if (Vector3.Distance(context.Self.position, context.StartPoint) <= context.State.DistanceGap)
        {
            context.Self.position = context.StartPoint;
            context.ScheduleStateChange(EnemyStateType.Idle);
        }

        if (Vector3.Distance(context.Self.position, context.Target.position) < context.State.FindDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Chase);
        }
    }
    public void Exit()
    {

    }
}
