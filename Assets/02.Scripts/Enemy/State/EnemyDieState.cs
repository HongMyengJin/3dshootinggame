using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx)
    {
        context = ctx;

        GameObject.Destroy(ctx.Self.gameObject);
    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
