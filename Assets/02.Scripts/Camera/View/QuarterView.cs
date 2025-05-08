using UnityEngine;

public class QuarterView : ICameraView
{
    private Transform player;

    public QuarterView(Transform cameraTransform, Transform player)
    {
        this.player = player;
        player.localRotation = Quaternion.identity;
        SettingCursor();
    }

    public void UpdateView()
    {
        Transform cameraTransform = CameraManager.Instance.GetCurrentCamera().transform;
        Vector3 offset = new Vector3(-6.0f * 0.8f, 8.0f * 0.8f, -6.0f * 0.8f);
        cameraTransform.position = player.position + offset;
        cameraTransform.rotation = Quaternion.Euler(45.0f, 45.0f, 0.0f);
    }

    public void SettingCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
