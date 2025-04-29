using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour, IEnemyContext, IEnemy
{

    // --- 컴포넌트 및 참조 --
    [SerializeField] protected Transform _self;                
    [SerializeField] protected Transform _target;              
    [SerializeField] protected NavMeshAgent _agent;            
    [SerializeField] protected CharacterController _controller;
    [SerializeField] protected EnemyStatSO _stat;
    [SerializeField] protected HealthComponent _healthComponent;
    [SerializeField] protected HealthBarController _healthBarController;
    [SerializeField] protected Animator _animator;

    // --- 상태 관련 ---
    protected readonly Dictionary<EnemyStateType, IEnemyState> stateMap = new();
    protected IEnemyState _currentState;                       
    protected EnemyStateType _currentType;                     
    protected EnemyStateType _sheduledChangeType;              
    protected Coroutine _scheduledTransition;                  

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
    public bool IsActive => gameObject.activeInHierarchy;
    public Transform Transform => transform;
    public Animator Animator => _animator;

    public EnemyStateType CurrentType => _currentType;
    protected virtual void Awake()
    {
        _startPosition = transform.position;
        _controller = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _stat.MoveSpeed;

        _animator = GetComponent<Animator>();

        if (_healthComponent != null && _healthBarController != null)
        {
            _healthBarController.Setup(transform, _healthComponent);
        }
    }
    protected virtual void Update()
    {
        _currentState?.Update();
    }

    protected void ChangeState(EnemyStateType next)
    {
        _currentState?.Exit();        
        if (stateMap.TryGetValue(next, out var nextState))
        {
            _currentState = nextState;
            _currentType = next;
            _currentState.Enter(this);
            Debug.Log($"상태 전환: {_currentType} -> {next}");
        }
        else
        {
            UnityEngine.Debug.LogWarning($"[ChangeState] {next} 상태는 이 Enemy에 등록되어 있지 않습니다.");
        }

    }

    public void ScheduleStateChange(EnemyStateType next, float delay)
    {
        if (stateMap.TryGetValue(next, out var nextState))
        {
            _scheduledTransition = StartCoroutine(DelayedChange(next, delay));
        }
        else
        {
            UnityEngine.Debug.LogWarning($"[ChangeState] {next} 상태는 이 Enemy에 등록되어 있지 않습니다.");
        }
    }

    public void SetDestination(Vector3 targetPosition)
    {
        Agent.SetDestination(targetPosition);
    }

    private IEnumerator DelayedChange(EnemyStateType next, float delay)
    {
        _sheduledChangeType = next;
        yield return new WaitForSeconds(delay);
        ChangeState(next);
    }

    public virtual void Initialize(Vector3 spawnPosition) // 소환될 때 초기화
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }
    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
    public void TakeDamage(Damage damage)
    {
        _healthComponent.TakeDamage(damage.Value);
        _health -= damage.Value;
        _knockbackDirection = damage.Dir;

        ChangeState(EnemyStateType.Damaged);
    }
}
