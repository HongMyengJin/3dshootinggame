using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YOffset = 10f;
    public float ProjectionSize = 5.0f;
    public Camera Camera;

    public CameraSO MinimapCameraSo;

    private void LateUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.y = YOffset;

        transform.position = newPosition;

        // 플레이어가 y축 회전한만큼 미니맵 카메라도 회전
        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90.0f;
        newEulerAngles.z = 0.0f;
        transform.eulerAngles = newEulerAngles;
    }

    public void ZoomInMinimap()
    {
        ProjectionSize -= MinimapCameraSo.OrthographicAddvalue;
        ProjectionSize = Mathf.Clamp(ProjectionSize, MinimapCameraSo.CameraMinOrthographicSize, MinimapCameraSo.CameraMaxOrthographicSize);
        Camera.orthographicSize = ProjectionSize;
    }

    public void ZoomOutMinimap()
    {
        ProjectionSize += MinimapCameraSo.OrthographicAddvalue;
        ProjectionSize = Mathf.Clamp(ProjectionSize, MinimapCameraSo.CameraMinOrthographicSize, MinimapCameraSo.CameraMaxOrthographicSize);
        Camera.orthographicSize = ProjectionSize;
    }
}
