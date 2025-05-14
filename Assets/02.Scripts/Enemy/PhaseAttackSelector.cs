using System.Collections.Generic;
using UnityEngine;

public class PhaseAttackSelector
{
    private readonly List<PhaseDataSO> _phases;
    private PhaseDataSO _currentPhase;
    private readonly Dictionary<EnemyAttackType, EnemyAttackStrategyBase> _strategyMap;
    private readonly DissolveController _shieldController;

    public PhaseAttackSelector(List<PhaseDataSO> phases, DissolveController shieldController)
    {
        _phases = phases;
        _shieldController = shieldController;
        _strategyMap = new Dictionary<EnemyAttackType, EnemyAttackStrategyBase>();

        InitializeStrategies();
    }


    private void InitializeStrategies()
    {
        _strategyMap[EnemyAttackType.Punch] = new EnemyPunchAttackStrategy();
        _strategyMap[EnemyAttackType.Throw] = new EnemyThrowAttackStrategy();
        _strategyMap[EnemyAttackType.Jump] = new EnemyJumpAttackStrategy();
        _strategyMap[EnemyAttackType.Shield] = new EnemyShieldDefenseStrategy(_shieldController);
    }

    public void UpdatePhase(float hpPercent)
    {
        foreach (PhaseDataSO phase in _phases)
        {
            if (hpPercent >= phase.minHpPercent && hpPercent <= phase.maxHpPercent)
            {
                _currentPhase = phase;
                Debug.Log($"현재 페이즈: {_currentPhase.name}");
                return;
            }
        }
    }

    public EnemyAttackStrategyBase SelectStrategy(IEnemyAttackContext context)
    {
        foreach (EnemyAttackType type in _currentPhase?.allowedAttacks)
        {
            var strategy = _strategyMap[type];
            if (strategy != null && strategy.CanUse(context))
                return strategy;
        }
        return null;
    }
}
