using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f;

    private float _rotationX = 0;

    private void Update()
    {
        // 1. ���콺 �Է��� �޴´�. (���콺�� Ŀ���� ������ ����)
        float mouseX = Input.GetAxis("Mouse X");

        // 2. ȸ���� �縸ŭ �������� ������.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;

        // 3. ȸ�� �������� ȸ����Ų��.
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
}
