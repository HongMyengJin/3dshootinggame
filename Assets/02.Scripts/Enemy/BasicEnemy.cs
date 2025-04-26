using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class BasicEnemy : EnemyBase, IDamageable, IEnemyIdleContext, IEnemyChaseContext, IEnemyPatrolContext, IEnemyReturnContext, IEnemyAttackContext, IEnemyDamagedContext, IEnemyDieContext
{
    [SerializeField] private Transform[] _patrolPoints;

    protected readonly Dictionary<EnemyStateType, IEnemyState> stateMap = new();


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
        stateMap.Add(EnemyStateType.Idle, EnemyStateFactory.Get(EnemyStateType.Idle));
        stateMap.Add(EnemyStateType.Chase, EnemyStateFactory.Get(EnemyStateType.Chase));
        stateMap.Add(EnemyStateType.Attack, EnemyStateFactory.Get(EnemyStateType.Attack));
        stateMap.Add(EnemyStateType.Damaged, EnemyStateFactory.Get(EnemyStateType.Damaged));
        stateMap.Add(EnemyStateType.Return, EnemyStateFactory.Get(EnemyStateType.Return));
        stateMap.Add(EnemyStateType.Patrol, EnemyStateFactory.Get(EnemyStateType.Patrol));
        stateMap.Add(EnemyStateType.Die, EnemyStateFactory.Get(EnemyStateType.Die));
    }


    protected override void Update()
    {
        currentState?.Update();
    }

    public void MoveToNextPatrolPoint()
    {
        patrolIndex = (patrolIndex + 1) % PatrolPoints.Length;
        Agent.SetDestination(PatrolPoints[patrolIndex].position);
    }
    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        _knockbackDirection = damage.Dir;

        ChangeState(EnemyStateType.Damaged);
    }
}