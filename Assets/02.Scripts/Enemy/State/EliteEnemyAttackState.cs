using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EliteEnemyAttackState : EnemyAttackStateBase
{
    private readonly PhaseAttackSelector _selector;
    private EnemyAttackStrategyBase _currentStrategy;
    private EnemyAttackStrategyBase _preStrategy;
    private EnemyAttackType _currentAttackType = EnemyAttackType.EnemyAttackTypeEnd;

    public EliteEnemyAttackState(List<PhaseDataSO> phaseDataList, DissolveController shieldController)
    {
        _selector = new PhaseAttackSelector(phaseDataList, shieldController);
    }

    public override void Enter(IEnemyContext ctx)
    {
        _currentStrategy = null;
        context = ctx as IEnemyAttackContext;
    }

    public override void Update()
    {
        _preStrategy = _currentStrategy;
        Debug.Log($"현재 공격 전략: {_currentStrategy} ");
        if (context == null) return;

        _selector.UpdatePhase(context.HpPercent);

        if (_currentStrategy == null || _currentStrategy.IsFinished())
        {
            _currentStrategy = _selector.SelectStrategy(context);

            bool shouldChase = _currentStrategy == null && Vector3.Distance(context.Self.position, context.Target.position) < context.State.AttackDistance;

            if (shouldChase)
                context.ScheduleStateChange(EnemyStateType.Chase);

            if (_preStrategy != _currentStrategy)
            {
                _currentStrategy?.Execute(context);
            }
        }

        _currentStrategy?.Update(context);
    }

    public override void LateUpdate()
    {
        
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