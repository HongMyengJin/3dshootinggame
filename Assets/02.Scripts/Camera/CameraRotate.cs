using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표: 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    // 구현 순서
    // 1. 마우스 입력을 받는다.
    // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
    // 3. 카메라를 그 방향으로 회전한다.

    public float RotationSpeed = 20f;

    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
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
        // 1. 마우스 입력을 받는다. (마우스의 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 회전한 양만큼 누적시켜 나간다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY -= mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f); // 상하 회전 제한

        // 3. 회전 방향으로 회전시킨다.
        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }
}

//Vector3 dir = new Vector3(-mouseY, mouseX, 0);

//// Todo: 마우스 좌표계와 화면 좌표계의 차이점을 알고, 잘 작동하도록 아래 한 줄의 코드를 잘~ 수정해보세요.

//// 3. 카메라를 그 방향으로 회전한다.
//// 새로운 위치 = 현재 위치 + 이동 속도 * 시간
//// 새로운 각도 = 현재 각도 + 회전 속도 * 시간
//transform.eulerAngles = transform.eulerAngles + dir * RotationSpeed * Time.deltaTime;

//// 회전의 상하 제한이 필요하다! (-90 ~ 90)
//Vector3 rotation = transform.eulerAngles;
//rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
//transform.eulerAngles = rotation;
//// 0 ~ 359도 사이
//// 누적해나가야 한다.