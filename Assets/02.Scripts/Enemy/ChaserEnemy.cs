using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class ChaserEnemy : EnemyBase, IDamageable, IEnemyChaseContext, IEnemyDamagedContext, IEnemyDieContext
{
    protected override void Awake()
    {
        base.Awake();
        InitializeStates();
        ChangeState(EnemyStateType.Follow);
    }
    public void InitializeStates()
    {
        stateMap.Add(EnemyStateType.Follow, new EnemyFollowState(new EnemyChaseStragegy()));
        stateMap.Add(EnemyStateType.Damaged, new EnemyDamagedState(new EnemyDamagedStragegy(), EnemyStateType.Follow));
        stateMap.Add(EnemyStateType.Die, new EnemyDieState(new EnemyDieStragegy()));
    }


    protected override void Update()
    {
        _currentState?.Update();
    }


    public override void TakeDamage(Damage damage)
    {
        _healthComponent.TakeDamage(damage.Value);
        _health -= damage.Value;
        _knockbackDirection = damage.Dir;

        ChangeState(EnemyStateType.Damaged);
    }
}