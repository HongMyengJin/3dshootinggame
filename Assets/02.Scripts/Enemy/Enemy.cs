using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 1. ���¸� ���������� �����Ѵ�.
    public enum EnemyState
    {
        Idle, // ���
        Patrol, // ����
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
    //private float _damagedTimer = 0.0f; // �� üũ��
    public float DeathTime = 0.2f; // �״� �ð�
    public float PatrolTime = 3.0f; // Idle���� üũ �ð�
    

    public Transform[] PatrolPositions;
    public int PatrolIndex = 0;

    private const float _distanceGap = 0.1f;
    private const float _nockBackTime = 0.5f;
    private float _nockBackMaxSpeed = 20.0f;

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
            case EnemyState.Patrol:
            {
                    Patrol();
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

    public void TakeDamage(Damage damage, Vector3 dir)
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
        StartCoroutine(Knockback(_nockBackTime, dir));
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

        // ���� Idle �ð��� 5�� ������
        StartCoroutine(Patrol_Coroutine());
    }

    public void Patrol()
    {
        // ���� ���������� ��������
        if(TargetFollow(PatrolPositions[PatrolIndex].position))
            PatrolIndex = (++PatrolIndex) % PatrolPositions.Length;
    }

    public bool TargetFollow(Vector3 Taget) // Follow �Ϸ� - true
    {
        Vector3 pos = transform.position;
        Vector3 dir = (Taget - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

        // ���� ���� ����
        float dis = Vector2.Distance(new Vector2(Taget.x, Taget.z), new Vector2(pos.x, pos.z));
        Debug.Log($"(���� Distance: {dis})");
        return (dis <= _characterController.minMoveDistance + _distanceGap);

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
        TargetFollow(_player.transform.position);
    }

    private void Return()
    {
        // �ൿ: �����Ѵ�.

        // ����: ���� ��ġ�� ��������� -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance + _distanceGap)
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

    private IEnumerator Patrol_Coroutine()
    {

        // �ڷ�ƾ ������� ����
        yield return new WaitForSeconds(PatrolTime);
        Debug.Log("������ȯ: Idle -> Patrol");
        CurrentState = EnemyState.Patrol;
    }

    private IEnumerator Knockback(float knockbackTime, Vector3 dir)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < knockbackTime)
        {
            float time = elapsedTime / knockbackTime;
            float value = Mathf.Lerp(0.0f, _nockBackMaxSpeed, time);
            _characterController.Move(dir * value * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ڷ�ƾ ������� ����
    }
    //private void Damaged()
    //{
    //    // �ൿ: ���� �ð� ���� �����ִٰ� ����
    //    _damagedTimer += Time.deltaTime;
    //    if(_damagedTimer >= DamagedTime)
    //    {
    //        _damagedTimer = 0.0f;
    //        Debug.Log("������ȯ: Damaged -> Trace");
    //        CurrentState = EnemyState.Trace;
    //    }
    //}

    private void Die()
    {
        // �ൿ: �״´�.
    }
}
