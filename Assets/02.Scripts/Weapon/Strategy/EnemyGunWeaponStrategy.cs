using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using static PlayerFire;

public class EnemyGunWeaponStrategy : IWeaponStrategy
{
    public Transform Self { get; }

    public EnemyGunWeaponStrategy()
    {
    }

    public void OnAttackInput()
    {
    }
    public void Attack()
    {
        
    }
    public void OnAttackAnimationEvent()
    {
        ShootAttack();
    }

    public void ShootAttack()
    {

    }
}
