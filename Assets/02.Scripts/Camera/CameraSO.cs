using UnityEngine;

[CreateAssetMenu(fileName = "CameraSO", menuName = "Scriptable Objects/CameraSO")]
public class CameraSO : ScriptableObject
{
    [SerializeField]
    public float CameraMaxOrthographicSize;
    public float CameraMinOrthographicSize;
    public float OrthographicAddvalue;
}
