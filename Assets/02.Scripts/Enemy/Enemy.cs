using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 1. ���¸� ���������� �����Ѵ�.
    public enum EnemyState
    {
        Idle, // ���
        Trace, // ����
        Return, // ����
        Attack, // ����
        Damaged, // �ǰ�
        Die // ���
    }

    // 2. ���� ���¸� �����Ѵ�.
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;             // �÷��̾�
    public float FindDistance = 5.0f;         // �÷��̾� �߰� ����
    public float AttackDistance = 2.5f;       // �÷��̾� ���� ����
    public float ReturnDistance = 10.0f;         // �� ���� ����

    private CharacterController _characterController;
    private Vector3 _startPosition;
    public float MoveSpeed = 3.3f;

    public float AttackCooltime = 2.0f;
    private float _attackTimer = 0.0f;

    public int Health = 100;
    public float DamagedTime = 1.0f; // ���� �ð�
    private float _damagedTimer = 0.0f; // �� üũ��
    public float DeathTime = 0.2f; // �״� �ð�

    private void Start()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // ���� ���� ���¿� ���� ���� �Լ��� ȣ���Ѵ�.
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
        // ����߰ų� ���ݹް� �ִ� ���̸�...
        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        Health -= damage.Value;

        if (Health <= 0)
        {
            CurrentState = EnemyState.Die;
            Debug.Log($"������ȯ: {CurrentState} -> Damaged");
            CurrentState = EnemyState.Die;
            StartCoroutine(Die_Coroutine());
            return;
        }

        Debug.Log($"������ȯ: {CurrentState} -> Damaged");

        CurrentState = EnemyState.Damaged;
        StartCoroutine(Damaged_Coroutine());
    }

    // 3. ���� �Լ����� �����Ѵ�.
    private void Idle()
    {
        // �ൿ: ������ �ִ´�.
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ: Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Trace()
    {

        // ����: �÷��̾�� �־����� -> Return
        if (Vector3.Distance(transform.position, _player.transform.position) >= ReturnDistance)
        {
            Debug.Log("������ȯ: Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }

        // ����: ���� ���� ��ŭ ����� ���� -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("������ȯ: Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // �ൿ: �÷��̾ �����Ѵ�.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        // �ൿ: �����Ѵ�.

        // ����: ���� ��ġ�� ��������� -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance)
        {
            Debug.Log("������ȯ: Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // ����: ���� ��ġ�� ��������� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ: Return -> Trace");
            CurrentState = EnemyState.Trace;
        }

        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // �ൿ: �÷��̾ �����Ѵ�.

        // ����: ���� �������� �־����� -> Trace
        // ����: ���� ��ġ�� ��������� -> Idle
        if (Vector3.Distance(transform.position, _startPosition) >= AttackDistance)
        {
            Debug.Log("������ȯ: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }
        
        // �ൿ: �÷��̾ �����Ѵ�.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            Debug.Log("�÷��̾� ����!");
            _attackTimer = 0;

        }

    }

    private IEnumerator Damaged_Coroutine()
    {

        // �ڷ�ƾ ������� ����
        yield return new WaitForSeconds(DamagedTime);
        Debug.Log("������ȯ: Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }
    private IEnumerator Die_Coroutine()
    {

        // �ڷ�ƾ ������� ����
        yield return new WaitForSeconds(DeathTime);
        Debug.Log("������ȯ: Damaged -> Die");
        CurrentState = EnemyState.Die;
        gameObject.SetActive(false);
    }

    private void Damaged()
    {
        // �ൿ: ���� �ð� ���� �����ִٰ� ����
        _damagedTimer += Time.deltaTime;
        if(_damagedTimer >= DamagedTime)
        {
            _damagedTimer = 0.0f;
            Debug.Log("������ȯ: Damaged -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Die()
    {
        // �ൿ: �״´�.
    }
}
