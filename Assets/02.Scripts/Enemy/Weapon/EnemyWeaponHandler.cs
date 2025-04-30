using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    private IWeaponStrategy _currentWeaponStrategy;
    private Dictionary<WeaponType, IWeaponStrategy> _weaponStrategies;

    private void Awake()
    {
        _weaponStrategies = new Dictionary<WeaponType, IWeaponStrategy>()
        {
            { WeaponType.Gun, new EnemyGunWeaponStrategy() }
        };
        SwitchWeapon(WeaponType.Gun);
    }
    public void OnAttackAnimationEvent()
    {
        _currentWeaponStrategy?.OnAttackAnimationEvent();
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
