using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour, IEnemyContext, IDamageable
{
    [SerializeField] private Transform _self;
    [SerializeField] private Transform _target;
    [SerializeField] private EnemyStatSO _stat;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private int _health;

    private int patrolIndex;
    private Vector3 _startPosition;
    private Vector3 _knockbackDirection;

    private IEnemyState currentState;
    private EnemyStateType currentType;

    private EnemyStateType sheduledChangeType;
    private Coroutine sheduledTransition;
    private Coroutine scheduledTransition;

    private NavMeshAgent _agent;               // 네비메쉬 에이전트
    public Transform Target => _target;
    public Transform Self => _self;
    public Vector3 StartPoint => _startPosition;
    public EnemyStatSO State => _stat;
    public NavMeshAgent Agent => _agent;
    public Transform[] PatrolPoints => _patrolPoints;
    public int PatrolIndex => patrolIndex;
    public int Health => _health;
    public CharacterController Controller => _controller;
    public Vector3 KnockbackDirection => _knockbackDirection;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _stat.MoveSpeed;

        _startPosition = transform.position;
        _controller = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeState(EnemyStateType.Idle);
    }

    private void Update()
    {
        currentState?.Update();
    }

    private void ChangeState(EnemyStateType next)
    {
        currentState?.Exit();
        currentState = EnemyStateFactory.Get(next);
        currentState.Enter(this);
        currentType = next;
        Debug.Log($"상태 전환: {currentType} -> {next}");

    }

    public void ScheduleStateChange(EnemyStateType next, float delay)
    {
        //if (scheduledTransition != null && sheduledChangeType == next)
        //{
        //    return;
        //}

        //if (scheduledTransition != null)
        //{
        //    StopCoroutine(scheduledTransition);
        //}

        scheduledTransition = StartCoroutine(DelayedChange(next, delay));
    }

    private IEnumerator DelayedChange(EnemyStateType next, float delay)
    {
        sheduledChangeType = next;
        yield return new WaitForSeconds(delay);
        ChangeState(next);
    }

    public new Coroutine StartCoroutine(IEnumerator routine) => base.StartCoroutine(routine);
    public new void StopCoroutine(Coroutine coroutine) => base.StopCoroutine(coroutine);

    public void MoveToNextPatrolPoint()
    {
        patrolIndex = (patrolIndex + 1) % PatrolPoints.Length;
        Agent.SetDestination(PatrolPoints[patrolIndex].position);
    }

    public void SetDestination(Vector3 targetPosition)
    {
        Agent.SetDestination(targetPosition);
    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        _knockbackDirection = damage.Dir;

        ChangeState(EnemyStateType.Damaged);
    }
}
