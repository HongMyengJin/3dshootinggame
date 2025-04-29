using UnityEngine;

public interface IEnemyStrategy<TContext> where TContext : IEnemyContext
{
    void Execute(TContext context);
    void Update(TContext context);
    void Exit(TContext context);
}
