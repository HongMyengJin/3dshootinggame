using System.Collections.Generic;
using UnityEngine;
public class EliteEnemyAttackState : EnemyAttackStateBase
{
    private EnemyAttackStrategyBase currentStrategy;

    public EliteEnemyAttackState()
    {
        strategies = new Dictionary<EnemyAttackType, EnemyAttackStrategyBase>
        {
            { EnemyAttackType.Punch, new EnemyPunchAttackStrategy() },
            { EnemyAttackType.Jump, new EnemyJumpAttackStrategy() },
            { EnemyAttackType.Throw, new EnemyThrowAttackStrategy() },
            { EnemyAttackType.Shield, new EnemyShieldDefenseStrategy() }
        };
    }
    private EnemyAttackType GetSelectAndExecuteStrategy()
    {
        float distance = Vector3.Distance(context.Self.position, context.Target.position);

        EnemyAttackType selectedType = EnemyAttackType.EnemyAttackTypeEnd;
        if (context.ShouldBlock()) selectedType = EnemyAttackType.Shield;
        else if (distance < 2f) selectedType = EnemyAttackType.Punch;
        else if (distance < 4f) selectedType = EnemyAttackType.Jump;
        else if (distance < 6f) selectedType = EnemyAttackType.Throw;

        return selectedType;
    }

    public override void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyAttackContext;
    }

    public override void Update()
    {
        if (context == null) return;

        currentStrategy?.Update(context);

        if (currentStrategy == null || currentStrategy.IsFinished())
        {
            EnemyAttackType selectedType = GetSelectAndExecuteStrategy();
            if (selectedType != EnemyAttackType.EnemyAttackTypeEnd &&
                strategies.TryGetValue(selectedType, out var strategy) && strategy.CanUse())
            {
                currentStrategy = strategy;
                currentStrategy.Execute(context);
            }
            else if(Vector3.Distance(context.Self.position, context.Target.position) > context.State.AttackDistance)
                context.ScheduleStateChange(EnemyStateType.Chase);
        }
    }

    public override void Exit()
    {
        currentStrategy?.Exit(context);
    }
}