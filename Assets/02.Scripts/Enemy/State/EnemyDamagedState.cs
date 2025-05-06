using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;
public class EnemyDamagedState : IEnemyState
{
    private IEnemyDamagedContext context;
    private IEnemyStrategy<IEnemyDamagedContext> damagedStrategy;
    private readonly EnemyStateType nextState;

    public EnemyDamagedState(IEnemyStrategy<IEnemyDamagedContext> damagedStrategy, EnemyStateType nextState)
    {
        this.damagedStrategy = damagedStrategy;
        this.nextState = nextState;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyDamagedContext;
        Animator animator = ctx.Animator;
        damagedStrategy.Execute(context);

        animator.SetTrigger("Damage");
        context.StartCoroutine(Stun(context, nextState));
    }
    public void Update()
    {
        if (context == null)
            return;
        damagedStrategy.Update(context);
    }
    public void Exit()
    {
        damagedStrategy.Exit(context);
    }

    private IEnumerator Stun(IEnemyDamagedContext context, EnemyStateType nextState)
    {
        // 코루틴 방식으로 변경
        context.Agent.isStopped = true;
        context.Agent.ResetPath();
        yield return new WaitForSeconds(context.State.DamagedStunTime);
        context.ScheduleStateChange(nextState);
    }

}
