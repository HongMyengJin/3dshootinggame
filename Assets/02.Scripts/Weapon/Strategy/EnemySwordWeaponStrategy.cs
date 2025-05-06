using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class EnemySwordWeaponStrategy : IWeaponStrategy
{
    private readonly WeaponHitBox _swordHitBox;
    public Transform Self { get; }

    public EnemySwordWeaponStrategy(WeaponHitBox weaponHitBox)
    {
        _swordHitBox = weaponHitBox;
    }

    public void OnAttackInput()
    {
    }
    public void Attack()
    {
        
    }
    public void OnAttackAnimationEvent()
    {
        _swordHitBox.Activate();
    }

    public void OffAttackAnimationEvent()
    {
        _swordHitBox.Deactivate();
    }
}
