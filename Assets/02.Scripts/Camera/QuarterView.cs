using UnityEngine;

public class QuarterView : ICameraView
{
    private Transform cameraTransform;
    private Transform player;

    public QuarterView(Transform cameraTransform, Transform player)
    {
        this.cameraTransform = cameraTransform;
        this.player = player;
    }

    public void UpdateView()
    {
        Vector3 offset = new Vector3(-6.0f, 8.0f, -6.0f);
        cameraTransform.position = player.position + offset;
        cameraTransform.rotation = Quaternion.Euler(45.0f, 45.0f, 0.0f);
    }
}
