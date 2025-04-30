using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FPSView : ICameraView
{
    private Transform cameraTransform;
    private Transform player;   

    private float yaw;
    private float pitch;

    private readonly Vector3 offset = new Vector3(0, 0.1f, 0.0f);

    public FPSView(Transform _cameraTransform, Transform _player)
    {
        this.cameraTransform = Camera.main.transform;
        this.player = _player;

        SettingCursor();
    }

    public void UpdateView()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -60.0f, 60.0f);

        player.rotation = Quaternion.Euler(0.0f, yaw, 0.0f);

        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        cameraTransform.position = player.position + offset;
        //cameraTransform.parent.transform.position = player.position + offset;
        //Vector3 cameraFoward = cameraTransform.forward;
        //cameraFoward.Normalize();
        //cameraFoward.y = 0.0f;

        //player.forward = cameraFoward;
    }

    public void SettingCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
