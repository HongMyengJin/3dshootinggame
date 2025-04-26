using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseStragegy : IEnemyStrategy<IEnemyChaseContext>
{
    public void Execute(IEnemyChaseContext context)
    {
        if (context == null)
            return;
    }
}
