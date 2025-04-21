using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f;

    private float _rotationX = 0;

    private void Update()
    {
        // 1. 마우스 입력을 받는다. (마우스의 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");

        // 2. 회전한 양만큼 누적시켜 나간다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;

        // 3. 회전 방향으로 회전시킨다.
        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
}
