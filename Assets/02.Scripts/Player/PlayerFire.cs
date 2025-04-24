using System.Data;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public enum ShootEnum
    {
        None,
        Load
    }

    public struct FireData
    {
        public Vector3 direction;
        public Ray ray;
        public RaycastHit? hitInfo;
    }

    // �ʿ� �Ӽ�
    // - �߻� ��ġ
    public GameObject FirePosition;
    // - ��ź ������
    public GameObject BombPrefab;
    // - ������ ��
    public float ThrowPower = 0.0f;

    public const float MaxThrowPower = 50.0f;

    public const float AddThrowSpeed = 20.0f;

    private Camera _mainCamera;

    public const float MaxLoadTime = 2.0f;
    public float CurLoadTime = 0.0f;

    public const float MaxCoolTime = 0.5f;
    public float CurCoolTime = 0.0f;

    public ShootEnum CurShootEnum;

    private void Start()
    {
        _mainCamera = Camera.main;

        CurShootEnum = ShootEnum.None;
    }

    private void Update()
    {
        CurCoolTime -= Time.deltaTime;
        // 2. ������ ��ư �Է� �ޱ�
        // - 0: ����, 1: ������, 2: ��

        // ���콺 �������� ���� �� + ���� ��� ������ ��ź ����
        if (Input.GetMouseButton(1) && BombUI.Instance.CheckUseBomb())
        {
            AddPower();
        }

        // ���콺 ������ �ø� ��
        if (Input.GetMouseButtonUp(1) && BombUI.Instance.CheckUseBomb())
        {
            BombAttack();
            ThrowPower = 0.0f;
            BombGaugeUpdate();
            BombUI.Instance.UseBomb();
        }

        // ��ǥ: ���콺�� ���� ��ư�� ������ ī�޶� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�. - �Ѿ� �߻�(������ ���)
        // 1. ���� ��ư �Է� �ޱ�
        if (Input.GetMouseButton(0) && CurCoolTime < 0.0f)
        {
            ShootAttack();
            Gun.Instance.Shoot();
        }

        LoadGun();
    }

    public void BombAttack()
    {
        // 3. �߻� ��ġ�� ����ź
        GameObject bomb = BombManager.Instance.UseBomb(FirePosition.transform.position);
        // 4. ������ ����ź�� ī�޶� �������� �������� �� ���ϱ�
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
        bombRigidbody.AddForce(_mainCamera.transform.forward * ThrowPower, ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);
    }

    public FireData CalculateFireData()
    {
        CameraViewType cameraViewType = CameraManager.Instance.GetCurrentViewType();
        Vector3 muzzlePos = FirePosition.transform.position;

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
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Vector3 dir = (hit.point - muzzlePos).normalized;
                        return new FireData { direction = dir, ray = new Ray(muzzlePos, dir), hitInfo = hit };
                    }
                    else
                    {
                        Vector3 dir = transform.forward;
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
            LazerManager.Instance.SettingLine(FirePosition.transform.position, hitInfo.point);

            Debug.Log($"���� �� ���� �±�: {hitInfo.collider.gameObject.tag}");

            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                Vector3 hitDir = -hitInfo.normal;
                hitDir.y = 0.0f;

                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                Damage damage = new Damage
                {
                    Value = 10,
                    From = this.gameObject
                };

                enemy.TakeDamage(damage, hitDir);
            }
        }

        CurShootEnum = ShootEnum.None;
        BulletUI.Instance.UpdateState(CurShootEnum);
        CurCoolTime = MaxCoolTime;
    }


    public void AddPower()
    {
        ThrowPower += Time.deltaTime * AddThrowSpeed;
        BombGaugeUpdate();
    }

    public void BombGaugeUpdate()
    {
        BombGaugeUI.Instance.Ratio = ThrowPower / MaxThrowPower;
        BombGaugeUI.Instance.UpdateValue();
    }

    public void LoadGun()
    {
        // ����
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CurShootEnum == ShootEnum.None) // ���� ����
            {
                CurLoadTime = MaxLoadTime;
            }
            else if (CurShootEnum == ShootEnum.Load) // ���� �� - ���
            {
                CurLoadTime = 0.0f;
            }

            CurShootEnum = CurShootEnum == ShootEnum.None ? ShootEnum.Load : ShootEnum.None;
            BulletUI.Instance.UpdateState(CurShootEnum);
        }

        if (CurShootEnum == ShootEnum.Load)
        {
            CurLoadTime -= Time.deltaTime;
            if (CurLoadTime < 0.0f)
            {
                // ���� �Ϸ�
                CurLoadTime = 0.0f;
                BulletManager.Instance.ResetBullet();
            }
        }
        BulletUI.Instance.UpdateBulletTimer(CurLoadTime);
    }
}
