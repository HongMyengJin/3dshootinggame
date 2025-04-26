using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackStragegy : IEnemyStrategy<IEnemyAttackContext>
{
    public void Execute(IEnemyAttackContext context)
    {
        if (context == null)
            return;
    }
}
