using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolStragegy : IEnemyStrategy<IEnemyPatrolContext>
{
    public void Execute(IEnemyPatrolContext context)
    {
        if (context == null || context.Agent == null)
            return;

        NavMeshAgent Agent = context.Agent;
        if (!Agent.pathPending && Agent.remainingDistance < context.State.DistanceGap)
        {
            context.MoveToNextPatrolPoint(); // 다음 경로 이동
        }
    }
}
