using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseStragegy : IEnemyStrategy<IEnemyChaseContext>
{
    public void Execute(IEnemyChaseContext context)
    {
        if (context == null)
            return;

        NavMeshAgent Agent = context.Agent;
        Agent.isStopped = false;
        Agent.updateRotation = true;
    }
    public void Update(IEnemyChaseContext context)
    {
        float speed = context.Agent.velocity.magnitude;
        context.Animator.SetFloat("MoveSpeed", speed);
        context.Agent.SetDestination(context.Target.transform.position);
    }

    public void Exit(IEnemyChaseContext context)
    {
        // 여기서 갑자기 방향이 틀어지네..
       //context.Animator.SetFloat("MoveSpeed", 0.0f);
        NavMeshAgent Agent = context.Agent;
        Agent.ResetPath();
        Agent.isStopped = true;
        Agent.updateRotation = false;
    }
}
