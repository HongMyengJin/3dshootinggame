using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolStragegy : IEnemyStrategy<IEnemyPatrolContext>
{
    public void Execute(IEnemyPatrolContext context)
    {
        if (context == null || context.Agent == null)
            return;

        NavMeshAgent Agent = context.Agent;
        float speed = Agent.velocity.magnitude;
        context.Animator.SetFloat("MoveSpeed", speed);

        if (!Agent.pathPending && Agent.remainingDistance < context.State.DistanceGap)
        {
            context.MoveToNextPatrolPoint(); // 다음 경로 이동
        }
    }
    public void Update(IEnemyPatrolContext context)
    {

    }
    public void Exit(IEnemyPatrolContext context)
    {
        NavMeshAgent agent = context.Agent;

        agent.isStopped = true;
        agent.ResetPath();
    }
}
