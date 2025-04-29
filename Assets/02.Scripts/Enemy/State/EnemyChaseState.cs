using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private IEnemyChaseContext context;
    private IEnemyStrategy<IEnemyChaseContext> chaseStrategy;
    public EnemyChaseState(IEnemyStrategy<IEnemyChaseContext> chaseStrategy)
    {
        this.chaseStrategy = chaseStrategy;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyChaseContext;
        chaseStrategy.Execute(context);
    }
    public void Update()
    {
        if (context == null)
            return;

        chaseStrategy.Update(context);
        float distance = Vector3.Distance(context.Self.position, context.Target.position);
        if (distance >= context.State.ReturnDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Return);
        }
        else if (distance < context.State.AttackDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Attack);
        }
    }
    public void Exit()
    {
        chaseStrategy.Exit(context);
    }
}
