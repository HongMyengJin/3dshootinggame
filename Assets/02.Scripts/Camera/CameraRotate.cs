using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotate : MonoBehaviour
{
    // ī�޶� ȸ�� ��ũ��Ʈ
    // ��ǥ: ���콺�� �����ϸ� ī�޶� �� �������� ȸ����Ű�� �ʹ�.
    // ���� ����
    // 1. ���콺 �Է��� �޴´�.
    // 2. ���콺 �Է����κ��� ȸ����ų ������ �����.
    // 3. ī�޶� �� �������� ȸ���Ѵ�.

    public float RotationSpeed = 20f;

    // ī�޶� ������ 0���������� �����Ѵٰ� ������ �����.
    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _rotationX = angles.y;
        _rotationY = angles.x;
    }


    private void Update()
    {
        // 1. ���콺 �Է��� �޴´�. (���콺�� Ŀ���� ������ ����)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. ȸ���� �縸ŭ �������� ������.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY -= mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f); // ���� ȸ�� ����

        // 3. ȸ�� �������� ȸ����Ų��.
        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }
}

//Vector3 dir = new Vector3(-mouseY, mouseX, 0);

//// Todo: ���콺 ��ǥ��� ȭ�� ��ǥ���� �������� �˰�, �� �۵��ϵ��� �Ʒ� �� ���� �ڵ带 ��~ �����غ�����.

//// 3. ī�޶� �� �������� ȸ���Ѵ�.
//// ���ο� ��ġ = ���� ��ġ + �̵� �ӵ� * �ð�
//// ���ο� ���� = ���� ���� + ȸ�� �ӵ� * �ð�
//transform.eulerAngles = transform.eulerAngles + dir * RotationSpeed * Time.deltaTime;

//// ȸ���� ���� ������ �ʿ��ϴ�! (-90 ~ 90)
//Vector3 rotation = transform.eulerAngles;
//rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
//transform.eulerAngles = rotation;
//// 0 ~ 359�� ����
//// �����س����� �Ѵ�.