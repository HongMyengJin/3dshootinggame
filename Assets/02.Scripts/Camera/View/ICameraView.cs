using UnityEngine;

// 시점별로 나누기 위한 카메라 인터페이스
public interface ICameraView
{
    void UpdateView();
    void SettingCursor();
}
