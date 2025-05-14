using UnityEngine;

public interface IWeaponStrategy
{ 
    void OnAttackInput();
    void Attack();
    void ToggleActiveStance();

    void OnAttackAnimationEvent();
    void OffAttackAnimationEvent();
}
