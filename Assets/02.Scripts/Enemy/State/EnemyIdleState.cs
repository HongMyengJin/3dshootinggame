using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyIdleState : IEnemyState
{
    private IEnemyIdleContext context;
    private IEnemyStrategy<IEnemyIdleContext> idleStrategy;
    public EnemyIdleState(IEnemyStrategy<IEnemyIdleContext> idleStrategy)
    {
        this.idleStrategy = idleStrategy;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyIdleContext;
    }
    public void Update()
    {
        if (context == null)
            return;

        idleStrategy.Execute(context);
        if (Vector3.Distance(context.Self.position, context.Target.position) < context.State.FindDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Chase);
            return;
        }

        context.ScheduleStateChange(EnemyStateType.Patrol, context.State.PatrolWaitTime);
    }
    public void Exit()
    {

    }

    }
