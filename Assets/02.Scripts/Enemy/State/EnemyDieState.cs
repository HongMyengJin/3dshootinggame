using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx)
    {
        context = ctx;

        context.ScheduleStateChange();
    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
