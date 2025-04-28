using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeaponStrategy : IWeaponStrategy
{
    private readonly MonoBehaviour _executionContext;
    private readonly Animator _animator;
    private readonly Transform _attacker;
    private readonly AttackDataSO _attackData;
    public SwordWeaponStrategy(Animator animator, Transform attacker, AttackDataSO attackData, MonoBehaviour executionContext)
    {
        _animator = animator;
        _attacker = attacker;
        _attackData = attackData;
        this._executionContext = executionContext;
    }

    public void OnAttackInput()
    {
        _animator.SetTrigger("Slash");
    }

    public void OnAttackAnimationEvent()
    {
        _executionContext.StartCoroutine(SwingAttackCoroutine());
    }
    public void Attack()
    {

    }
    public void AttackOnce()
    {

        var enemies = EnemySpawner.Instance.GetActiveEnemies();

        if (enemies == null || enemies.Count == 0) 
            return;

        Vector3 origin = _attacker.position;
        Vector3 forward = _attacker.forward;
        float minDot = Mathf.Cos(_attackData.attackAngle * 0.5f * Mathf.Deg2Rad);
        float rangeSqr = _attackData.attackRange * _attackData.attackRange;

        foreach (IEnemy enemy in enemies)
        {
            if (enemy == null || enemy.CurrentType == EnemyStateType.Damaged) continue;

            // 거리 체크 (원을 기준)
            Vector3 toEnemy = enemy.Transform.position - origin;
            if (toEnemy.sqrMagnitude > rangeSqr)
                continue;

            // 각도 체크 (부채꼴 범위 체크)
            float dot = Vector3.Dot(forward, toEnemy.normalized);
            if (dot < minDot)
                continue;

            if (enemy.Transform.TryGetComponent<IDamageable>(out var damageable))
            {
                Damage damage = new Damage
                {
                    Value = _attackData.attackDamage,
                    From = _attacker.gameObject,
                    Dir = toEnemy.normalized
                };

                damageable.TakeDamage(damage);
            }
        }
    }

    private IEnumerator SwingAttackCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < _attackData.swingDuration)
        {
            AttackOnce(); 
            yield return new WaitForSeconds(_attackData.hitInterval);
            elapsed += _attackData.hitInterval;
        }
    }
}