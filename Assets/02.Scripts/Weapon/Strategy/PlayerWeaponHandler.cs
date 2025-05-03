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
            { WeaponType.Sword, new SwordWeaponStrategy(_animator, transform, _swordAttackData, this) },
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

    //private void LateUpdate()
    //{
    //    //Ray ray = CameraManager.Instance.GetCurrentCamera().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //    //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    //    //Debug.DrawRay(CameraManager.Instance.GetCurrentCamera().transform.position, CameraManager.Instance.GetCurrentCamera().transform.forward * 100f, Color.blue);
    //}
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
            _weaponInstances[weaponType].transform.SetParent(_weaponSocket);
            _weaponInstances[weaponType].transform.localPosition = Vector3.zero;
            _weaponInstances[weaponType].transform.localRotation = Quaternion.identity;
            // _weaponInstances[weaponType].transform.position = _weaponSocket.position;

            _weaponInstances[weaponType]?.SetActive(true);
            _currentWeaponType = weaponType;
        }
        else
        {
            Debug.LogWarning($"무기 전략을 찾을 수 없음: {weaponType}");
        }
    }

    //public void OnAttackHit()
    //{
    //    _currentWeaponStrategy?.Attack();
    //}


    private void LateUpdate()
    {
        if(_IsAttack)
        {
            ShootAttack();
            _IsAttack = false;
        }
    }

    public void OnAttack()
    {
        _IsAttack = true;
    }
    //public FireData CalculateFireData()
    //{
    //    Camera cam = CameraManager.Instance.GetCurrentCamera();
    //    CameraViewType cameraViewType = CameraManager.Instance.GetCurrentViewType();
    //    Vector3 muzzlePos = _muzzlePosition.position;

    //    switch (cameraViewType)
    //    {
    //        case CameraViewType.FPS:
    //            {
    //                Ray camRay = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
    //                Vector3 fireDir = camRay.direction;

    //                Ray fireRay = new Ray(muzzlePos, fireDir);
    //                Physics.Raycast(fireRay, out RaycastHit hit, 200f);
    //                Vector3 targetPoint = hit.point;
    //                Vector3 direction = (targetPoint - muzzlePos).normalized;

    //                return new FireData
    //                {
    //                    direction = direction,
    //                    ray = fireRay,
    //                    hitInfo = hit,
    //                };
    //            }
    //        case CameraViewType.TPS:
    //            {
    //                Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
    //                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
    //                {
    //                    Vector3 dir = (hit.point - muzzlePos).normalized;
    //                    return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = hit };
    //                }
    //                else
    //                {
    //                    Vector3 dir = ray.direction;
    //                    return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = null };
    //                }
    //            }
    //        case CameraViewType.QuaterView:
    //            {
    //                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    //                float t = (muzzlePos.y - ray.origin.y) / ray.direction.y;
    //                Vector3 targetPos = ray.origin + ray.direction * t;
    //                if (Physics.Raycast(ray, out RaycastHit hit))
    //                {
    //                    Vector3 dir = (hit.point - muzzlePos).normalized;
    //                    return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = hit };
    //                }
    //                else
    //                {
    //                    Vector3 dir = _muzzlePosition.forward;
    //                    return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = null };
    //                }
    //            }
    //    }

    //    return new FireData { direction = Vector3.zero, ray = new Ray(muzzlePos, Vector3.forward), hitInfo = null };
    //}
    //public void ShootAttack()
    //{
    //    FireData fire = CalculateFireData();

    //    if (!fire.hitInfo.HasValue)
    //        return;

    //    RaycastHit hitInfo = fire.hitInfo.Value;

    //    BulletManager.Instance.UseBullet(hitInfo.point);
    //    LazerManager.Instance.SettingLine(_muzzlePosition.position, hitInfo.point);

    //    if (hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
    //    {
    //        Vector3 hitDir = -hitInfo.normal;
    //        hitDir.y = 0.0f;

    //        Damage damage = new Damage
    //        {
    //            Value = 10,
    //            From = _muzzlePosition.gameObject,
    //            Dir = hitDir
    //        };

    //        damageable.TakeDamage(damage);
    //    }
    //}


    public void ShootAttack()
    {
        FireData fire = CalculateFireData();

        if (!fire.hitInfo.HasValue)
            return;

        RaycastHit hitInfo = fire.hitInfo.Value;

        // 시각/효과 처리
        BulletManager.Instance.UseBullet(hitInfo.point);
        LazerManager.Instance.SettingLine(_muzzlePosition.position, hitInfo.point);

        // 데미지 처리
        if (hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Vector3 hitDir = -hitInfo.normal;
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

        // fallback
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
