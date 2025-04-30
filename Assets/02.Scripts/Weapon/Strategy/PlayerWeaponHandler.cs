using UnityEngine;
using System.Collections.Generic;

public enum WeaponType
{
    Gun,
    Sword,
    Bomb,
    WeaponTypeEnd
}

public class PlayerWeaponHandler : MonoBehaviour
{
    private IWeaponStrategy _currentWeaponStrategy;

    [SerializeField] private Transform _weaponSocket;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<WeaponDataSO> _weaponDataAssets;
    [SerializeField] private AttackDataSO _swordAttackData;

    private Dictionary<WeaponType, IWeaponStrategy> _weaponStrategies;
    private Dictionary<WeaponType, GameObject> _weaponInstances;
    private Dictionary<KeyCode, WeaponType> _keyWeaponMappings;
    private WeaponType _currentWeaponType = WeaponType.Gun;

    private void Awake()
    {
        _weaponInstances = new Dictionary<WeaponType, GameObject>((int)WeaponType.WeaponTypeEnd);
        foreach (WeaponDataSO data in _weaponDataAssets)
        {
            if (data.weaponPrefab == null)
                continue;
            GameObject weapon = Instantiate(data.weaponPrefab, _weaponSocket);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.SetActive(false);

            
            _weaponInstances[data.weaponType] = weapon;
        }

        _weaponStrategies = new Dictionary<WeaponType, IWeaponStrategy>()
        {
            { WeaponType.Gun, new PlayerGunWeaponStrategy(_animator, _weaponInstances[WeaponType.Gun].transform.Find("MuzzlePosition")) },
            { WeaponType.Sword, new SwordWeaponStrategy(_animator, transform, _swordAttackData, this) },
            { WeaponType.Bomb, new BombWeaponStrategy(_animator, _weaponSocket, gameObject) }
        };

        _keyWeaponMappings = new Dictionary<KeyCode, WeaponType>()
        {
            { KeyCode.Alpha1, WeaponType.Gun },
            { KeyCode.Alpha2, WeaponType.Sword },
            { KeyCode.Alpha3, WeaponType.Bomb },
        };

    }
    private void Update()
    {
        HandleAttackInput();
        HandleWeaponSwitchInput();
    }

    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentWeaponStrategy?.OnAttackInput();
        }
    }
    public void OnAttackAnimationEvent()
    {
        _currentWeaponStrategy?.OnAttackAnimationEvent(); 
    }

    private void HandleWeaponSwitchInput()
    {
        foreach (var mapping in _keyWeaponMappings)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                SwitchWeapon(mapping.Value);
                break;
            }
        }
    }

    private void SwitchWeapon(WeaponType weaponType)
    {
        if (_weaponStrategies.TryGetValue(weaponType, out var strategy))
        {
            _currentWeaponStrategy = strategy;
            _weaponInstances[_currentWeaponType]?.SetActive(false);
            if (!_weaponInstances.ContainsKey(weaponType))
            {
                return;
            }
            Transform grip = _weaponInstances[weaponType].transform.Find("grip");

            Vector3 offset = _weaponInstances[weaponType].transform.position - grip.position;
            _weaponInstances[weaponType].transform.position = _weaponSocket.position + offset;

            _weaponInstances[weaponType]?.SetActive(true);
            _currentWeaponType = weaponType;
        }
        else
        {
            Debug.LogWarning($"무기 전략을 찾을 수 없음: {weaponType}");
        }
    }

    public void OnAttackHit()
    {
        _currentWeaponStrategy?.Attack();
    }
}
