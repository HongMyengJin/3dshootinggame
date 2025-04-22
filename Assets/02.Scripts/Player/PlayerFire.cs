using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // �ʿ� �Ӽ�
    // - �߻� ��ġ
    public GameObject FirePosition;
    // - ��ź ������
    public GameObject BombPrefab;
    // - ������ ��
    public float ThrowPower = 0.0f;

    public const float MaxThrowPower = 50.0f;

    public const float AddThrowSpeed = 20.0f;

    public ParticleSystem BulletEffect;

    private Camera _mainCamera;



    private void Start()
    {
        _mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // 2. ������ ��ư �Է� �ޱ�
        // - 0: ����, 1: ������, 2: ��

        // ���콺 �������� ���� �� + ���� ��� ������ ��ź ����
        if(Input.GetMouseButton(1) && BombUI.Instance.CheckUseBomb())
        {
            AddPower();
        }

        // ���콺 ������ �ø� ��
        if (Input.GetMouseButtonUp(1))
        {
            BombAttack();
            ThrowPower = 0.0f;
            BombGaugeUpdate();
            BombUI.Instance.UseBomb();
        }

        // ��ǥ: ���콺�� ���� ��ư�� ������ ī�޶� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�. - �Ѿ� �߻�(������ ���)
        // 1. ���� ��ư �Է� �ޱ�
        if (Input.GetMouseButtonDown(0))
        {
            ShootAttack();
        }


        // Ray: ������(���� ��ġ, ����)
        // RayCast: �������� �߻�
        // RayCastHit: �������� ��ü�� �ε����ٸ� �� ������ �����ϴ� ����ü
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

    public void ShootAttack()
    {
        // 2. ���̸� �����ϰ� �߻� ��ġ�� ���� ������ ����
        Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);

        // 3. ���̿� �ε��� ��ü�� ������ ������ ������ ����
        RaycastHit hitInfo = new RaycastHit();

        // 4. ���̸� �߻��� ����,                 
        bool isHit = Physics.Raycast(ray, out hitInfo);
        if (isHit) // ���� �����Ͱ� �ִٸ�(�ε����ٸ�) �ǰ� ����Ʈ ����(ǥ��)
        {
            // �ǰ� ����Ʈ ����(ǥ��)
            BulletEffect.transform.position = hitInfo.point;
            BulletEffect.Play();
        }
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
}
