using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyChaseStragegy : IEnemyStrategy<IEnemyChaseContext>
{
    public void Execute(IEnemyChaseContext context)
    {
        if (context == null || context.Agent == null)
            return;

        NavMeshAgent Agent = context.Agent;
        Agent.isStopped = false;
        Agent.updateRotation = true;
    }
    public void Update(IEnemyChaseContext context)
    {
        if (context == null || context.Agent == null)
            return;

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
        if (context == null || context.Agent == null)
            return;

        context.StartCoroutine(SmoothStop(context.Animator));

        NavMeshAgent Agent = context.Agent;
        Agent.ResetPath();
        Agent.isStopped = true;
        Agent.updateRotation = false;
    }

    public IEnumerator SmoothStop(Animator animator)
    {
        float speed = animator.GetFloat("MoveSpeed");
        while (speed > 0.01f)
        {
            speed = Mathf.Lerp(speed, 0f, Time.deltaTime * 5f);
            animator.SetFloat("MoveSpeed", speed);
            yield return null;
        }
        animator.SetFloat("MoveSpeed", 0f);
    }
}
