using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    private Transform _muzzlePosition;
    private bool _IsAttack = false;
    private bool _IsShootAttack = false;

    private Dictionary<WeaponType, IWeaponStrategy> _weaponStrategies;
    private Dictionary<WeaponType, GameObject> _weaponInstances;
    private Dictionary<KeyCode, WeaponType> _keyWeaponMappings;
    private WeaponType _currentWeaponType = WeaponType.Gun;

    public Transform WeaponSocket => _weaponSocket;

   

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
            { WeaponType.Gun, new PlayerGunWeaponStrategy(_animator, _weaponInstances[WeaponType.Gun].transform.Find("GripPoint/MuzzlePosition"), OnAttack) },
            { WeaponType.Sword, new SwordWeaponStrategy(_animator, transform, _swordAttackData, this, _weaponInstances[WeaponType.Sword].GetComponent<WeaponHitBox>()) },
            { WeaponType.Bomb, new BombWeaponStrategy(_animator, _weaponSocket, gameObject) }
        };

        _keyWeaponMappings = new Dictionary<KeyCode, WeaponType>()
        {
            { KeyCode.Alpha1, WeaponType.Gun },
            { KeyCode.Alpha2, WeaponType.Sword },
            { KeyCode.Alpha3, WeaponType.Bomb },
        };
        _muzzlePosition = _weaponInstances[WeaponType.Gun].transform.Find("GripPoint/MuzzlePosition");
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
            _weaponInstances[weaponType].transform.SetParent(_weaponSocket);
            _weaponInstances[weaponType].transform.localPosition = Vector3.zero;
            _weaponInstances[weaponType].transform.localRotation = Quaternion.identity;

            _weaponInstances[weaponType]?.SetActive(true);
            _currentWeaponType = weaponType;
        }
        else
        {
            Debug.LogWarning($"무기 전략을 찾을 수 없음: {weaponType}");
        }
    }
    private void LateUpdate()
    {
        if(_IsShootAttack)
        {
            ShootAttack();
            _IsShootAttack = false;
            _IsAttack = false;
        }
    }
    public void OnAttack()
    {
        _IsShootAttack = true;
    }
    public bool IsAttack() // 현재 공격 중 or Aim 상태일 때(조준 중)
    {
        return _IsAttack || _animator.GetBool("Aim");
    }
    public void OnAttackAnimationEvent()
    {
        Debug.Log("Enemy - OnAttackAnimationEvent 호출");
        _currentWeaponStrategy?.OnAttackAnimationEvent();
        _IsAttack = true;
    }

    public void OffAttackAnimationEvent()
    {
        Debug.Log("Enemy - OffAttackAnimationEvent 호출");
        _currentWeaponStrategy?.OffAttackAnimationEvent();
        _IsAttack = false;
    }

    public void ShootAttack()
    {
        FireData fire = CalculateFireData();

        if (!fire.hitInfo.HasValue)
            return;

        RaycastHit hitInfo = fire.hitInfo.Value;

        BulletManager.Instance.UseBullet(hitInfo.point);
        LazerManager.Instance.SettingLine(_muzzlePosition.position, hitInfo.point);

        if (hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Vector3 hitDir = (hitInfo.point - _muzzlePosition.position).normalized;
            // Vector3 hitDir = -hitInfo.normal;
            hitDir.y = 0.0f;

            Damage damage = new Damage
            {
                Value = 10,
                From = _muzzlePosition.gameObject,
                Dir = hitDir
            };

            damageable.TakeDamage(damage);
        }
    }

    public FireData CalculateFireData()
    {
        Camera cam = CameraManager.Instance.GetCurrentCamera();
        Vector3 muzzlePos = _muzzlePosition.position;
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        CameraViewType viewType = CameraManager.Instance.GetCurrentViewType();

        switch (viewType)
        {
            case CameraViewType.FPS:
            case CameraViewType.TPS:
                return CalculateFireDataFromCenterRay(cam, screenCenter, muzzlePos, viewType == CameraViewType.FPS ? 200f : 100f);

            case CameraViewType.QuaterView:
                return CalculateFireDataFromMouse(cam, muzzlePos);
        }

        return new FireData { direction = Vector3.forward, ray = new Ray(muzzlePos, Vector3.forward), hitInfo = null };
    }

    private FireData CalculateFireDataFromCenterRay(Camera cam, Vector3 screenPoint, Vector3 muzzlePos, float maxDistance)
    {
        Ray camRay = cam.ScreenPointToRay(screenPoint);

        if (Physics.Raycast(camRay, out RaycastHit hit, maxDistance))
        {
            Vector3 dir = (hit.point - muzzlePos).normalized;
            return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = hit };
        }
        else
        {
            Vector3 dir = camRay.direction;
            return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = null };
        }
    }

    private FireData CalculateFireDataFromMouse(Camera cam, Vector3 muzzlePos)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dir = (hit.point - muzzlePos).normalized;
            return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = hit };
        }
        else
        {
            float t = (muzzlePos.y - ray.origin.y) / ray.direction.y;
            Vector3 targetPos = ray.origin + ray.direction * t;
            Vector3 dir = (targetPos - muzzlePos).normalized;

            return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = null };
        }
    }



}
