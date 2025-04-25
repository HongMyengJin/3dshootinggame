using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx) => context = ctx;
    public void Update()
    {
        if (Vector3.Distance(context.Self.position, context.Target.position) >= context.State.AttackDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Chase);
        }
        // 나중에 공격 코드 추가
    }
    public void Exit()
    {

    }
}
