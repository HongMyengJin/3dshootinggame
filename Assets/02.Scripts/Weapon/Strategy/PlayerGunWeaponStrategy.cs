using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using static PlayerFire;

public class PlayerGunWeaponStrategy : IWeaponStrategy
{
    private readonly Animator _animator;
    private readonly Transform _firePoint;
    private bool _aim = false;

    public Transform Self { get; }

    public PlayerGunWeaponStrategy(Animator animator, Transform firePoint)
    {
        this._animator = animator;

        if (firePoint == null)
            Debug.Log("쏘는 포인트 없음");
        this._firePoint = firePoint;
    }

    public void OnAttackInput()
    {
        _aim = !_aim;
        _animator.SetBool("Aim", _aim);
    }
    public void Attack()
    {
        
    }
    public void OnAttackAnimationEvent()
    {
        ShootAttack();
        // Gun.Instance.Shoot();
    }

    public FireData CalculateFireData()
    {
        CameraViewType cameraViewType = CameraManager.Instance.GetCurrentViewType();
        Vector3 muzzlePos = _firePoint.position;

        switch (cameraViewType)
        {
            case CameraViewType.FPS:
                {
                    Vector3 dir = Camera.main.transform.forward;
                    return new FireData
                    {
                        direction = dir,
                        ray = new Ray(muzzlePos, dir),
                        hitInfo = null
                    };
                }
            case CameraViewType.TPS:
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
                    if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                    {
                        Vector3 dir = (hit.point - muzzlePos).normalized;
                        return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = hit };
                    }
                    else
                    {
                        Vector3 dir = ray.direction;
                        return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = null };
                    }
                }
            case CameraViewType.QuaterView:
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float t = (muzzlePos.y - ray.origin.y) / ray.direction.y;
                    Vector3 targetPos = ray.origin + ray.direction * t;
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Vector3 dir = (hit.point - muzzlePos).normalized;
                        return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = hit };
                    }
                    else
                    {
                        Vector3 dir = _firePoint.forward;
                        return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = null };
                    }
                }
        }

        return new FireData { direction = Vector3.zero, ray = new Ray(muzzlePos, Vector3.forward), hitInfo = null };
    }
    public void ShootAttack()
    {
        FireData fire = CalculateFireData();
        RaycastHit hitInfo;

        bool isHit = false;

        if (fire.hitInfo.HasValue)
        {
            hitInfo = fire.hitInfo.Value;
            isHit = true;
        }
        else if (Physics.Raycast(fire.ray, out hitInfo))
        {
            isHit = true;
            fire.hitInfo = hitInfo;
        }

        if (isHit)
        {

            hitInfo = fire.hitInfo.Value;
            BulletManager.Instance.UseBullet(hitInfo.point);
            LazerManager.Instance.SettingLine(_firePoint.position, hitInfo.point);

            Debug.Log($"현재 총 맞은 태그: {hitInfo.collider.gameObject.tag}");

            if (hitInfo.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Vector3 hitDir = -hitInfo.normal;
                hitDir.y = 0.0f;

                Damage damage = new Damage
                {
                    Value = 10,
                    From = _firePoint.gameObject,
                    Dir = hitDir
                };

                damageable.TakeDamage(damage);
            }
        }

        //CurShootEnum = ShootEnum.None;
        //BulletUI.Instance.UpdateState(CurShootEnum);
        //CurCoolTime = MaxCoolTime;
    }
}
