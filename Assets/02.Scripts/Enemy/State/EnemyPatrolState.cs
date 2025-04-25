using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx) => context = ctx;
    public void Update()
    {
        NavMeshAgent Agent = context.Agent;
        if (!Agent.pathPending && Agent.remainingDistance < context.State.DistanceGap)
        {
            context.MoveToNextPatrolPoint(); // 다음 경로 이동
        }

        if (Vector3.Distance(context.Self.position, context.Target.position) < context.State.FindDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Chase);
            return;
        }

    }
    public void Exit()
    {
        NavMeshAgent Agent = context.Agent;

        Agent.isStopped = true;
        Agent.ResetPath();
    }
}

