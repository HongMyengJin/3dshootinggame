using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class BasicEnemy : BaseEnemy, IDamageable
{
    [SerializeField] private Transform[] _patrolPoints;

    private int patrolIndex;

    public Transform[] PatrolPoints => _patrolPoints;
    public int PatrolIndex => patrolIndex;

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStateType.Idle);
    }

    protected override void Update()
    {
        currentState?.Update();
    }

    public new Coroutine StartCoroutine(IEnumerator routine) => base.StartCoroutine(routine);
    public new void StopCoroutine(Coroutine coroutine) => base.StopCoroutine(coroutine);

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