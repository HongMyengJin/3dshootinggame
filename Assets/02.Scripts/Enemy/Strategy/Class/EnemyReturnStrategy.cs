using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyReturnStragegy : IEnemyStrategy<IEnemyReturnContext>
{
    public void Execute(IEnemyReturnContext context)
    {
        if (context == null)
            return;

        context.Agent.SetDestination(context.StartPoint);
    }
}
