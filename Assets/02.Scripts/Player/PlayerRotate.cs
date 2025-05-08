using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 0.1f;

    private float _rotationX = 0;

    private Quaternion _targetRotation;
    private Animator _animator;

    private float _rotateValue;

    private float _cameraYaw;
    private float _cameraPitch;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (CameraManager.Instance.GetCurrentViewType() != CameraViewType.QuaterView) // 쿼터뷰가 아닐 때
        {
            Transform cameraTransform = CameraManager.Instance.GetCurrentCamera().transform;


            Vector3 lookDir = cameraTransform.forward;
            lookDir.y = 0f;
            lookDir.Normalize(); // 중요!

            if (lookDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
                Debug.Log($"{lookDir} LookDir (정규화 후): 회전중~~");
            }
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
                _rotateValue = Quaternion.Angle(transform.rotation, _targetRotation) / 180.0f;
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * 5.0f);
                if(_rotateValue > 0.1f)
                    _animator.SetFloat("MoveSpeed", _rotateValue);
            }
        }
    }

    private void LateUpdate()
    {

    }
}
