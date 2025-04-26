using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR;
public class EnemyDamagedState : IEnemyState
{
    private IEnemyDamagedContext context;
    private IEnemyStrategy<IEnemyDamagedContext> damagedStrategy;
    public EnemyDamagedState(IEnemyStrategy<IEnemyDamagedContext> damagedStrategy)
    {
        this.damagedStrategy = damagedStrategy;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyDamagedContext;
        damagedStrategy.Execute(context);
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
