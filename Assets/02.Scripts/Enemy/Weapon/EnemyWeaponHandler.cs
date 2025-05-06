using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponHitBox swordHitbox;

    private IWeaponStrategy _currentWeaponStrategy;
    private Dictionary<WeaponType, IWeaponStrategy> _weaponStrategies;

    private void Awake()
    {
        _weaponStrategies = new Dictionary<WeaponType, IWeaponStrategy>()
        {
            { WeaponType.Sword, new EnemySwordWeaponStrategy(swordHitbox) }
        };
        SwitchWeapon(WeaponType.Sword);
    }
    public void OnAttackAnimationEvent()
    {
        Debug.Log("Enemy - OnAttackAnimationEvent 호출");
        _currentWeaponStrategy?.OnAttackAnimationEvent();
    }

    public void OffAttackAnimationEvent()
    {
        Debug.Log("Enemy - OffAttackAnimationEvent 호출");
        _currentWeaponStrategy?.OffAttackAnimationEvent();
    }

    private void SwitchWeapon(WeaponType weaponType)
    {
        if (_weaponStrategies.TryGetValue(weaponType, out var strategy))
        {
            _currentWeaponStrategy = strategy;
        }
        else
        {
            Debug.LogWarning($"무기 전략을 찾을 수 없음: {weaponType}");
        }
    }
}
