using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum CameraViewType { FPS, TPS, QuaterView }
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _fpsCamera;

    private bool _canControl = false;
    public static CameraManager Instance { get; private set; }
    public static event Action<CameraViewType> OnViewTypeChanged;
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
    private Dictionary<CameraViewType, Camera> _cameraMap;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerWeaponHandler playerWeaponHandler = player.GetComponent<PlayerWeaponHandler>();
        PlayerView playerView = player.GetComponent<PlayerView>();
        views = new Dictionary<CameraViewType, ICameraView>
        {
            { CameraViewType.FPS, new FPSView( playerWeaponHandler.WeaponSocket, player) },
            { CameraViewType.TPS, new TPSView( player.transform) },
            { CameraViewType.QuaterView, new QuarterView(Camera.main.transform, player.transform) },
        };

        _cameraMap = new Dictionary<CameraViewType, Camera>
        {
            { CameraViewType.FPS, _fpsCamera },
            { CameraViewType.TPS, _mainCamera },
            { CameraViewType.QuaterView, _mainCamera },
        };

        SetView(CameraViewType.FPS);
        SwitchCamera(CameraViewType.FPS);

    }

    public void SetView(CameraViewType type)
    {
        if (!views.ContainsKey(type))
        {
            return;
        }

        Cursor.lockState = (type == CameraViewType.QuaterView ? CursorLockMode.None : CursorLockMode.Locked);

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

    public void EnableResetCamera()
    {
        foreach(var camera in _cameraMap.Values)
        {
            camera.enabled = false;
        }
    }

    public void SwitchCamera(CameraViewType cameraType)
    {
        EnableResetCamera();
        if (_cameraMap.ContainsKey(cameraType))
        {
            _cameraMap[cameraType].enabled = true;
            currentViewType = cameraType;

            OnViewTypeChanged?.Invoke(cameraType);
        }

        
    }

    public Camera GetCurrentCamera()
    {
        if (_cameraMap.TryGetValue(currentViewType, out var camera))
        {
            return camera;
        }
        return null;
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
