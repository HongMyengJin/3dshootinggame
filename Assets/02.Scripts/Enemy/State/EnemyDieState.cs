using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private IEnemyDieContext context;
    private IEnemyStrategy<IEnemyDieContext> dieStrategy;
    public EnemyDieState(IEnemyStrategy<IEnemyDieContext> dieStrategy)
    {
        this.dieStrategy = dieStrategy;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyDieContext;
        dieStrategy.Execute(context);
    }
    public void Update()
    {
        if (context == null)
            return;
    }
    public void Exit()
    {

    }
}
