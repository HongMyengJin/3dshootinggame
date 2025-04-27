using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollowState : IEnemyState
{
    private IEnemyChaseContext context;
    private IEnemyStrategy<IEnemyChaseContext> chaseStrategy;
    public EnemyFollowState(IEnemyStrategy<IEnemyChaseContext> chaseStrategy)
    {
        this.chaseStrategy = chaseStrategy;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyChaseContext;
    }
    public void Update()
    {
        if (context == null)
            return;

        chaseStrategy.Execute(context);
        float distance = Vector3.Distance(context.Self.position, context.Target.position);
    }
    public void Exit()
    {

    }
}
