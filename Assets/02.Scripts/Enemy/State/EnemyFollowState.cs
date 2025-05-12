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
        chaseStrategy.Execute(context);
    }
    public void Update()
    {
        if (context == null)
            return;
        chaseStrategy.Update(context);
    }
    public void LateUpdate()
    {
    }
    public void Exit()
    {
        chaseStrategy.Exit(context);
    }
}
