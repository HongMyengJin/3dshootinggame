using System;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public struct FireData
{
    public Vector3 direction;
    public Ray ray;
    public RaycastHit? hitInfo;
}
public class PlayerGunWeaponStrategy : IWeaponStrategy
{
    private readonly Action _onFire;
    private readonly Animator _animator;
    private readonly Transform _firePoint;
    private bool _aim = false;

    public Transform Self { get; }

    private bool _shoot = false;

    public PlayerGunWeaponStrategy(Animator animator, Transform firePoint, Action fireCallback)
    {
        this._animator = animator;

        if (firePoint == null)
            Debug.Log("쏘는 포인트 없음");
        this._firePoint = firePoint;
        this._onFire = fireCallback;
    }

    public void OnAttackInput()
    {
        _aim = !_aim;
        _animator.SetBool("Aim", _aim);
    }
    public void Attack()
    {
        
    }
    public void OnAttackAnimationEvent()
    {
        _onFire?.Invoke();
    }
}
