using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EliteEnemyAttackState : EnemyAttackStateBase
{
    private readonly PhaseAttackSelector _selector;
    private EnemyAttackStrategyBase _currentStrategy;
    private EnemyAttackStrategyBase _preStrategy;
    // private EnemyAttackType _currentAttackType = EnemyAttackType.EnemyAttackTypeEnd;
    private float _lastUsedTime = 0.0f;
    private float _cooldown = 5.0f;
    public bool CanUse()
    {
        bool use = (Time.time - _lastUsedTime) > _cooldown;
        Debug.Log($"사용 가능: {use} , 쿨타임: {_cooldown}, 지난 시간: {_lastUsedTime}, 계산 시간: {(Time.time - _lastUsedTime)}");
        return use;
    }

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

        if (_currentStrategy == null || (_currentStrategy.IsFinished() && CanUse()))
        {
            _currentStrategy = _selector.SelectStrategy(context);
            if( _currentStrategy != null)
            {
                _currentStrategy.Execute(context);
                _lastUsedTime = Time.time;
                Debug.Log($"현재 전략: {_currentStrategy.GetType().Name}");
            }
            else
            {
                if(Vector3.Distance(context.Self.position, context.Target.position) < context.State.FindDistance)
                    context.ScheduleStateChange(EnemyStateType.Chase);
                else
                    context.ScheduleStateChange(EnemyStateType.Patrol);

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

    //public EnemyAttackType GetCurrentAttackType()
    //{
    //    return _currentAttackType;
    //}
}