using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerState : IPlayerState
{
    protected IPlayerMoveStrategy moveStrategy;

    private static readonly Dictionary<CameraViewType, IPlayerMoveStrategy> strategyMap = new Dictionary<CameraViewType, IPlayerMoveStrategy>
    {
        { CameraViewType.FPS,           new PlayerFPSMoveStrategy() },
        { CameraViewType.TPS,           new PlayerTPSMoveStrategy() },
        { CameraViewType.QuaterView,    new PlayerQuarterViewMoveStrategy() }
    };

    protected PlayerState()
    {
        
    }

    public virtual void Enter()
    {
        CameraManager.OnViewTypeChanged += OnViewTypeChanged;
        CameraViewType currentViewType = CameraManager.Instance != null
        ? CameraManager.Instance.GetCurrentViewType()
        : CameraViewType.FPS;
        OnViewTypeChanged(currentViewType);
    }

    public virtual void Exit()
    {
        CameraManager.OnViewTypeChanged -= OnViewTypeChanged;
    }

    public abstract void Update();


    private void OnViewTypeChanged(CameraViewType viewType)
    {
        moveStrategy = strategyMap.TryGetValue(viewType, out var strategy)
            ? strategy : new PlayerIdleMoveStrategy();
    }
}