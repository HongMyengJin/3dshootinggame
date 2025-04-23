using System.Data;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public enum ShootEnum
    {
        None,
        Load
    }

    // 필요 속성
    // - 발사 위치
    public GameObject FirePosition;
    // - 폭탄 프리팹
    public GameObject BombPrefab;
    // - 던지는 힘
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

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CurCoolTime -= Time.deltaTime;
        // 2. 오른쪽 버튼 입력 받기
        // - 0: 왼쪽, 1: 오른쪽, 2: 휠

        // 마우스 오른쪽이 눌릴 때 + 현재 사용 가능한 폭탄 존재
        if (Input.GetMouseButton(1) && BombUI.Instance.CheckUseBomb())
        {
            AddPower();
        }

        // 마우스 오른쪽 올릴 때
        if (Input.GetMouseButtonUp(1) && BombUI.Instance.CheckUseBomb())
        {
            BombAttack();
            ThrowPower = 0.0f;
            BombGaugeUpdate();
            BombUI.Instance.UseBomb();
        }

        // 목표: 마우스의 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총을 발사하고 싶다. - 총알 발사(레이저 방식)
        // 1. 왼쪽 버튼 입력 받기
        if (Input.GetMouseButton(0) && CurCoolTime < 0.0f)
        {
            ShootAttack();
            Gun.Instance.Shoot();
        }

        LoadGun();
    }

    public void BombAttack()
    {
        // 3. 발사 위치에 수류탄
        GameObject bomb = BombManager.Instance.UseBomb(FirePosition.transform.position);
        // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
        bombRigidbody.AddForce(_mainCamera.transform.forward * ThrowPower, ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);
    }

    public void ShootAttack()
    {
        // 2. 레이를 생성하고 발사 위치와 진행 방향을 설정
        Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);

        // 3. 레이와 부딪힌 물체의 정보를 저장할 변수를 생성
        RaycastHit hitInfo = new RaycastHit();

        // 4. 레이를 발사한 다음,                 
        bool isHit = Physics.Raycast(ray, out hitInfo);
        if (isHit) // ㄴ에 데이터가 있다면(부딪혔다면) 피격 이펙트 생성(표시)
        {
            // 피격 이펙트 생성(표시)
            BulletManager.Instance.UseBullet(hitInfo.point);
            LazerManager.Instance.SettingLine(FirePosition.transform.position, hitInfo.point);

            Debug.Log($"현재 총 맞은 태그: {hitInfo.collider.gameObject.tag}");
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                Vector3 hitDir = -hitInfo.normal;
                hitDir.y = 0.0f;
                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                Damage damage = new Damage();

                damage.Value = 10;
                damage.From = this.gameObject;

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
        // 장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CurShootEnum == ShootEnum.None) // 장전 시작
            {
                CurLoadTime = MaxLoadTime;
            }
            else if (CurShootEnum == ShootEnum.Load) // 장전 중 - 취소
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
                // 장전 완료
                CurLoadTime = 0.0f;
                BulletManager.Instance.ResetBullet();
            }
        }
        BulletUI.Instance.UpdateBulletTimer(CurLoadTime);
    }
}
