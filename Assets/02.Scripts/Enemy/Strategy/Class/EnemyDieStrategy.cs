using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyDieStragegy : IEnemyStrategy<IEnemyDieContext>
{
    public void Execute(IEnemyDieContext context)
    {
        if (context == null)
            return;
        GameObject.Destroy(context.Self.gameObject);
    }
}
