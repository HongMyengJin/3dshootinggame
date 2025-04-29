using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class CommonEnemy : EnemyBase, IDamageable, IEnemyIdleContext, IEnemyChaseContext, IEnemyPatrolContext, IEnemyReturnContext, IEnemyAttackContext, IEnemyDamagedContext, IEnemyDieContext
{
    [SerializeField] private Transform[] _patrolPoints;

    private int patrolIndex;

    public Transform[] PatrolPoints => _patrolPoints;
    public int PatrolIndex => patrolIndex;

    protected override void Awake()
    {
        base.Awake();
        InitializeStates();
        ChangeState(EnemyStateType.Idle);
    }

    public void InitializeStates()
    {
        stateMap.Add(EnemyStateType.Idle, new EnemyIdleState(new EnemyIdleStrategy()));
        stateMap.Add(EnemyStateType.Chase, new EnemyChaseState(new EnemyChaseStragegy()));
        stateMap.Add(EnemyStateType.Attack, new EnemyAttackState(new EnemyAttackStragegy()));
        stateMap.Add(EnemyStateType.Damaged, new EnemyDamagedState(new EnemyDamagedStragegy(), EnemyStateType.Chase));
        stateMap.Add(EnemyStateType.Return, new EnemyReturnState(new EnemyReturnStragegy()));
        stateMap.Add(EnemyStateType.Patrol, new EnemyPatrolState(new EnemyPatrolStragegy()));
        stateMap.Add(EnemyStateType.Die, new EnemyDieState(new EnemyDieStragegy()));
    }


    protected override void Update()
    {
        _currentState?.Update();
    }

    public void MoveToNextPatrolPoint()
    {
        patrolIndex = (patrolIndex + 1) % PatrolPoints.Length;
        Agent.SetDestination(PatrolPoints[patrolIndex].position);
    }
    //public void TakeDamage(Damage damage)
    //{
    //    _healthComponent.TakeDamage(damage.Value);
    //    _health -= damage.Value;
    //    _knockbackDirection = damage.Dir;

    //    ChangeState(EnemyStateType.Damaged);
    //}
}