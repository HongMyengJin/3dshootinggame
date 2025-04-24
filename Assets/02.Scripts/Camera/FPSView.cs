using UnityEngine;

public class FPSView : ICameraView
{
    private Transform cameraTransform;
    private Transform player;

    private float yaw;
    private float pitch;

    public FPSView(Transform _cameraTransform, Transform _player)
    {
        this.cameraTransform = _cameraTransform;
        this.player = _player;

        yaw = player.eulerAngles.y;
    }

    public void UpdateView()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -80.0f, 80.0f);

        cameraTransform.position = player.position + Vector3.up * 1.6f;
        cameraTransform.rotation = Quaternion.Euler(-pitch, yaw, 0.0f);
    }
}
