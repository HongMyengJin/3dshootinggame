using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public bool JumpPressed { get; private set; }
    public void ResetJump() => JumpPressed = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            CameraManager.Instance.SetView(CameraViewType.FPS);
            CameraManager.Instance.SwitchCamera(CameraViewType.FPS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CameraManager.Instance.SetView(CameraViewType.TPS);
            CameraManager.Instance.SwitchCamera(CameraViewType.TPS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraManager.Instance.SetView(CameraViewType.QuaterView);
            CameraManager.Instance.SwitchCamera(CameraViewType.QuaterView);
        }

        JumpPressed = Input.GetKeyDown(KeyCode.Space);
    }
}
