using UnityEngine;

public class GunWeaponStrategy : IWeaponStrategy
{

    private readonly Animator _animator;
    private readonly Transform _firePoint;

    public GunWeaponStrategy(Animator animator, Transform firePoint)
    {
        this._animator = animator;
        this._firePoint = firePoint;
    }

    public void OnAttackInput()
    {
        _animator.SetTrigger("Shoot");
    }
    public void Attack()
    {
        
    }
    public void OnAttackAnimationEvent()
    {

    }
}
