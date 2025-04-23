using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 1. 상태를 열거형으로 정의한다.
    public enum EnemyState
    {
        Idle, // 대기
        Trace, // 추적
        Return, // 복귀
        Attack, // 공격
        Damaged, // 피격
        Die // 사망
    }

    // 2. 현재 상태를 지정한다.
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;             // 플레이어
    public float FindDistance = 5.0f;         // 플레이어 발견 범위
    public float AttackDistance = 2.5f;       // 플레이어 공격 범위
    public float ReturnDistance = 10.0f;         // 적 복귀 범위

    private CharacterController _characterController;
    private Vector3 _startPosition;
    public float MoveSpeed = 3.3f;

    public float AttackCooltime = 2.0f;
    private float _attackTimer = 0.0f;

    public int Health = 100;
    public float DamagedTime = 1.0f; // 경직 시간
    private float _damagedTimer = 0.0f; // ㄴ 체크기
    public float DeathTime = 0.2f; // 죽는 시간

    private void Start()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // 나의 현재 상태에 따라 상태 함수를 호출한다.
        switch (CurrentState)
        {
            case EnemyState.Idle:
            {
                    Idle();
                    break;
            }
            case EnemyState.Trace:
            {
                    Trace();
                    break;
            }
            case EnemyState.Return:
            {
                    Return();
                    break;
            }
            case EnemyState.Attack:
            {
                    Attack();
                    break;
            }
        }
    }

    public void TakeDamage(Damage damage)
    {
        // 사망했거나 공격받고 있는 중이면...
        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        Health -= damage.Value;

        if (Health <= 0)
        {
            CurrentState = EnemyState.Die;
            Debug.Log($"상태전환: {CurrentState} -> Damaged");
            CurrentState = EnemyState.Die;
            StartCoroutine(Die_Coroutine());
            return;
        }

        Debug.Log($"상태전환: {CurrentState} -> Damaged");

        CurrentState = EnemyState.Damaged;
        StartCoroutine(Damaged_Coroutine());
    }

    // 3. 상태 함수들을 구현한다.
    private void Idle()
    {
        // 행동: 가만히 있는다.
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Trace()
    {

        // 전이: 플레이어와 멀어지면 -> Return
        if (Vector3.Distance(transform.position, _player.transform.position) >= ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }

        // 전이: 공격 범위 만큼 가까워 지면 -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // 행동: 플레이어를 추적한다.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        // 행동: 복귀한다.

        // 전이: 시작 위치와 가까워지면 -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // 전이: 시작 위치와 가까워지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            CurrentState = EnemyState.Trace;
        }

        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // 행동: 플레이어를 공격한다.

        // 전이: 공격 범위보다 멀어지면 -> Trace
        // 전이: 시작 위치와 가까워지면 -> Idle
        if (Vector3.Distance(transform.position, _startPosition) >= AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }
        
        // 행동: 플레이어를 공격한다.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            Debug.Log("플레이어 공격!");
            _attackTimer = 0;

        }

    }

    private IEnumerator Damaged_Coroutine()
    {

        // 코루틴 방식으로 변경
        yield return new WaitForSeconds(DamagedTime);
        Debug.Log("상태전환: Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }
    private IEnumerator Die_Coroutine()
    {

        // 코루틴 방식으로 변경
        yield return new WaitForSeconds(DeathTime);
        Debug.Log("상태전환: Damaged -> Die");
        CurrentState = EnemyState.Die;
        gameObject.SetActive(false);
    }

    private void Damaged()
    {
        // 행동: 일정 시간 동안 멈춰있다가 공격
        _damagedTimer += Time.deltaTime;
        if(_damagedTimer >= DamagedTime)
        {
            _damagedTimer = 0.0f;
            Debug.Log("상태전환: Damaged -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Die()
    {
        // 행동: 죽는다.
    }
}
