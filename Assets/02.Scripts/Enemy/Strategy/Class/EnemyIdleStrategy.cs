using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleStrategy : IEnemyStrategy<IEnemyIdleContext>
{
    public void Execute(IEnemyIdleContext context)
    {
        if (context == null)
            return;
    }
    public void Update(IEnemyIdleContext context)
    {

    }
    public void Exit(IEnemyIdleContext context)
    {

    }
}