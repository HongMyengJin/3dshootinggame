using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private IEnemyAttackContext context;
    private IEnemyStrategy<IEnemyAttackContext> attackStrategy;

    public EnemyAttackState(IEnemyStrategy<IEnemyAttackContext> attackStrategy)
    {
        this.attackStrategy = attackStrategy;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyAttackContext;
        attackStrategy.Execute(context);
    }
    public void Update()
    {
        if (context == null)
            return;

        attackStrategy.Update(context);
        if (Vector3.Distance(context.Self.position, context.Target.position) >= context.State.AttackDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Chase);
        }
    }
    public void Exit()
    {
        attackStrategy.Exit(context);
    }
}
