using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
        Transform selfTransform = context.Self;
        Quaternion selfRotation = context.Self.rotation;
        selfRotation.x = 0.0f;

        float speed = context.Agent.velocity.magnitude;
        context.Animator.SetFloat("MoveSpeed", speed);
        context.Agent.SetDestination(context.Target.transform.position);

        selfTransform.rotation = selfRotation;
    }

    public void Exit(IEnemyChaseContext context)
    {
        context.Animator.SetFloat("MoveSpeed", 0.0f);
        NavMeshAgent Agent = context.Agent;
        Agent.ResetPath();
        Agent.isStopped = true;
        Agent.updateRotation = false;
    }
}
