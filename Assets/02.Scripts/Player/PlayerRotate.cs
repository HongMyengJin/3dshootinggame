using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 200.0f;

    private float _rotationX = 0;

    private Quaternion _targetRotation;
    private void Update()
    {
        if (CameraManager.Instance.GetCurrentViewType() != CameraViewType.QuaterView)
        {
            // 1. 마우스 입력을 받는다. (마우스의 커서의 움직임 방향)
            float mouseX = Input.GetAxis("Mouse X");

            // 2. 회전한 양만큼 누적시켜 나간다.
            _rotationX += mouseX * RotationSpeed * Time.deltaTime;

            // 3. 회전 방향으로 회전시킨다.
            transform.eulerAngles = new Vector3(0, _rotationX, 0);
        }
        else
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    Vector3 clickPosition = hitInfo.point;
                    Vector3 direction = (clickPosition - transform.position);
                    direction.y = 0; // 수평 회전만
                    if (direction.sqrMagnitude > 0.01f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        _targetRotation = targetRotation;
                    }
                }
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * 5.0f);
            }
        }
    }
}
