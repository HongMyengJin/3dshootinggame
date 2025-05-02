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
        
        views = new Dictionary<CameraViewType, ICameraView>
        {
            // 타입에 따른 값들 할당
            { CameraViewType.FPS, new FPSView(Camera.main.transform, playerWeaponHandler.WeaponSocket, player) },
            { CameraViewType.TPS, new TPSView(Camera.main.transform, player.transform) },
            { CameraViewType.QuaterView, new QuarterView(Camera.main.transform, player.transform) },
        };

        _cameraMap = new Dictionary<CameraViewType, Camera>
        {
            { CameraViewType.FPS, _fpsCamera },
            { CameraViewType.TPS, _mainCamera },  // 같은 카메라 공유
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

    public void SwitchCamera(CameraViewType cameraType)
    {
        foreach (var (type, camera) in _cameraMap)
        {
            camera.enabled = type == cameraType;
        }
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
