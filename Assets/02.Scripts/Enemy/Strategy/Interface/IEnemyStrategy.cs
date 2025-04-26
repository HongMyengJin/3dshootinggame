using UnityEngine;

public interface IEnemyStrategy<TContext> where TContext : IEnemyContext
{
    void Execute(TContext context);
}
