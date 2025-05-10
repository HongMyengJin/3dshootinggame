using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class EliteEnemy : EnemyBase, IDamageable, IEnemyIdleContext, IEnemyChaseContext, IEnemyPatrolContext, IEnemyReturnContext, IEnemyAttackContext, IEnemyDamagedContext, IEnemyDieContext
{
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private DissolveController _shieldController;

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
        stateMap.Add(EnemyStateType.Attack, new EliteEnemyAttackState(_shieldController));
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
    public override void TakeDamage(Damage damage)
    {
        EliteEnemyAttackState eliteEnemyAttackState = _currentState as EliteEnemyAttackState;

        if (_currentType == EnemyStateType.Attack 
            && eliteEnemyAttackState != null 
            && eliteEnemyAttackState.GetCurrentAttackType() == EnemyAttackType.Shield)
        {
            return; // 현재 실드 상태이면 데미지 X
        }

        _healthComponent.TakeDamage(damage.Value);
        _health -= damage.Value;
        _knockbackDirection = damage.Dir;

        ChangeState(EnemyStateType.Damaged);
    }

    public bool ShouldBlock()
    {
        return PlayerManager.Instance.IsAttack();
    }
    private void LateUpdate()
    {
        _currentState?.LateUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"현재 트리거 중~ {other.name} ");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"현재 콜리전 중~ {collision.gameObject.name} ");
    }
}