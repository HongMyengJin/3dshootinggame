using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeaponStrategy : IWeaponStrategy
{
    private readonly WeaponHitBox _swordHitBox;

    private readonly MonoBehaviour _executionContext;
    private readonly Animator _animator;
    private readonly Transform _attacker;
    private readonly AttackDataSO _attackData;
    public SwordWeaponStrategy(Animator animator, Transform attacker, AttackDataSO attackData, MonoBehaviour executionContext, WeaponHitBox swordHitBox)
    {
        _animator = animator;
        _attacker = attacker;
        _attackData = attackData;
        this._executionContext = executionContext;
        _swordHitBox = swordHitBox;
    }

    public void OnAttackInput()
    {
        _animator.SetTrigger("Slash");
    }

    public void OnAttackAnimationEvent()
    {
        _swordHitBox.Activate();
        Debug.Log("플레이어 검 - OnAttackAnimationEvent: 켜짐");
    }
    public void OffAttackAnimationEvent()
    {
        _swordHitBox.Deactivate();
        Debug.Log("플레이어 검 - OnAttackAnimationEvent: 꺼짐");
    }
    public void Attack()
    {

    }
}