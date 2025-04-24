using UnityEngine;

public class TPSView : ICameraView
{
    private Transform cameraTransform;
    private Transform player;

    private float yaw;
    private float pitch;

    private readonly Vector3 offset = new Vector3(0, 2f, -4f);

    public TPSView(Transform _cameraTransform, Transform _player)
    {
        cameraTransform = _cameraTransform;
        player = _player;

        yaw = player.eulerAngles.y;
    }

    public void UpdateView()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -40.0f, 60.0f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        cameraTransform.position = player.position + rotation * offset;
        cameraTransform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
