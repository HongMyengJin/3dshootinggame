using UnityEngine;

public interface IWeaponStrategy
{
    void OnAttackInput();
    void Attack();

    void OnAttackAnimationEvent();
}
