using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour, IEnemyContext
{
    // --- 컴포넌트 및 참조 --
    [SerializeField] protected Transform _self;                
    [SerializeField] protected Transform _target;              
    [SerializeField] protected NavMeshAgent _agent;            
    [SerializeField] protected CharacterController _controller;
    [SerializeField] protected EnemyStatSO _stat;

    // --- 상태 관련 ---
    Dictionary<EnemyStateType, IEnemyState> _stateMap = new();
    protected IEnemyState currentState;                       
    protected EnemyStateType currentType;                     
    protected EnemyStateType sheduledChangeType;              
    protected Coroutine scheduledTransition;                  

    // --- 전투 및 위치 데이터 ---
    [SerializeField] protected int _health;                   
    protected Vector3 _startPosition;                         
    protected Vector3 _knockbackDirection;                    

    // --- 프로퍼티: 외부 접근용 ---
    public Transform Self => _self;
    public Transform Target => _target;
    public NavMeshAgent Agent => _agent;
    public CharacterController Controller => _controller;
    public EnemyStatSO State => _stat;
    public int Health => _health;

    public Vector3 StartPoint => _startPosition;
    public Vector3 KnockbackDirection => _knockbackDirection;

    protected virtual void Awake()
    {
        _startPosition = transform.position;
        _controller = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _stat.MoveSpeed;
    }
    protected virtual void Update()
    {
        currentState?.Update();
    }

    protected void ChangeState(EnemyStateType next)
    {
        currentState?.Exit();
        currentState = EnemyStateFactory.Get(next);
        currentState.Enter(this);
        currentType = next;
        Debug.Log($"상태 전환: {currentType} -> {next}");

    }

    public void ScheduleStateChange(EnemyStateType next, float delay)
    {
        scheduledTransition = StartCoroutine(DelayedChange(next, delay));
    }

    public void SetDestination(Vector3 targetPosition)
    {
        Agent.SetDestination(targetPosition);
    }

    private IEnumerator DelayedChange(EnemyStateType next, float delay)
    {
        sheduledChangeType = next;
        yield return new WaitForSeconds(delay);
        ChangeState(next);
    }
}
