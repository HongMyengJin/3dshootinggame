using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum CameraViewType { FPS, TPS, QuaterView }
public class CameraManager : MonoBehaviour
{
    private bool _canControl = false;
    public static CameraManager Instance { get; private set; }

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

    private ICameraView currentView;
    private CameraViewType currentViewType;
    private Dictionary<CameraViewType, ICameraView> views;

    private void Start()
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        views = new Dictionary<CameraViewType, ICameraView>
        {
            // 타입에 따른 값들 할당
            { CameraViewType.FPS, new FPSView(Camera.main.transform, _player.transform) },
            { CameraViewType.TPS, new TPSView(Camera.main.transform, _player.transform) },
            { CameraViewType.QuaterView, new QuarterView(Camera.main.transform, _player.transform) },
        };

        SetView(CameraViewType.QuaterView);
    }

    public void SetView(CameraViewType type)
    {
        if (!views.ContainsKey(type))
        {
            return;
        }

        Cursor.lockState = (type == CameraViewType.QuaterView ? CursorLockMode.None : CursorLockMode.Locked);

        currentViewType = type;
        currentView = views[type];
    }

    public CameraViewType GetCurrentViewType()
    {
        return currentViewType;
    }

    private void LateUpdate()
    {
        if (!_canControl)
            return;
        currentView?.UpdateView();
    }

    public void EnableControl()
    {
        _canControl = true;
    }

    public void DisableControl()
    {
        _canControl = false;
    }
}
