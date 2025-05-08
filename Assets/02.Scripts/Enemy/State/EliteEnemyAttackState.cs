using System.Collections.Generic;
using UnityEngine;
public class EliteEnemyAttackState : EnemyAttackStateBase
{
    private EnemyAttackStrategyBase _currentStrategy;
    private EnemyAttackType _currentAttackType = EnemyAttackType.EnemyAttackTypeEnd;

    public EliteEnemyAttackState(ShieldController shieldController)
    {
        strategies = new Dictionary<EnemyAttackType, EnemyAttackStrategyBase>
        {
            { EnemyAttackType.Punch, new EnemyPunchAttackStrategy() },
            { EnemyAttackType.Jump, new EnemyJumpAttackStrategy() },
            { EnemyAttackType.Throw, new EnemyThrowAttackStrategy() },
            { EnemyAttackType.Shield, new EnemyShieldDefenseStrategy(shieldController) }
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

        _currentStrategy?.Update(context);

        if (_currentStrategy == null || _currentStrategy.IsFinished())
        {
            EnemyAttackType selectedType = GetSelectAndExecuteStrategy();
            if (selectedType != EnemyAttackType.EnemyAttackTypeEnd &&
                strategies.TryGetValue(selectedType, out var strategy) && strategy.CanUse())
            {
                _currentStrategy = strategy;
                _currentAttackType = selectedType;
                _currentStrategy.Execute(context);
            }
            else if(Vector3.Distance(context.Self.position, context.Target.position) > context.State.AttackDistance)
                context.ScheduleStateChange(EnemyStateType.Chase);
        }
    }

    public override void Exit()
    {
        _currentStrategy?.Exit(context);
    }

    public EnemyAttackType GetCurrentAttackType()
    {
        return _currentAttackType;
    }
}