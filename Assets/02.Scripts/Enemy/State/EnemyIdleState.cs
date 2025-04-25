using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyIdleState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx) => context = ctx;
    public void Update()
    {
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
