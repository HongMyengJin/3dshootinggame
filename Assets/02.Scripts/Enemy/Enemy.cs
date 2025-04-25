using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour, IEnemyContext
{
    [SerializeField] private EnemyStatSO _stat;
    [SerializeField] private Transform _target;

    [SerializeField] private int _health;
    [SerializeField] private float _attackTimer;

    public Transform Target => _target;
    public EnemyStatSO State => _stat;
    

    private IEnemyState currentState;
    private EnemyStateType currentType;
    private Coroutine sheduledTransition;
    
    private CharacterController _characterController;
    private NavMeshAgent _agent;               // 네비메쉬 에이전트
    private Vector3 _startPosition;

    public Transform[] PatrolPositions;
    public int PatrolIndex = 0;

    private Coroutine scheduledTransition;


    public void ChangeState(EnemyStateType next)
    {
        currentState?.Exit();
        currentState = EnemyStateFactory.Get(next);
        currentState.Enter(this);
        currentType = next;
        Debug.Log($"상태 전환: {currentType} -> {next}");

    }

    public void ScheduleStateChange(EnemyStateType next, float delay)
    {
        if (scheduledTransition != null)
            StopCoroutine(scheduledTransition);

        scheduledTransition = StartCoroutine(DelayedChange(next, delay));
    }

    private IEnumerator DelayedChange(EnemyStateType next, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(next);
    }

    public new Coroutine StartCoroutine(IEnumerator routine) => StartCoroutine(routine);
    public new void StopCoroutine(Coroutine coroutine) => StopCoroutine(coroutine);


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _stat.MoveSpeed;

        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

}
