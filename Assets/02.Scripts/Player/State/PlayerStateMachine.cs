using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private Dictionary<PlayerStateType, PlayerState> _stateMap;
    private IPlayerState _currentState;

    public PlayerStateMachine(PlayerController player, Animator animator)
    {
        _stateMap = new Dictionary<PlayerStateType, PlayerState>
        {
            { PlayerStateType.Walk, new PlayerMoveState(player) },
            { PlayerStateType.Damage, new PlayerDamagedState(player) }
        };

        _currentState = _stateMap[PlayerStateType.Walk];
        _currentState.Enter();
    }

    public void ChangeState(PlayerStateType stateType)
    {
        _currentState?.Exit();
        _currentState = _stateMap[stateType];
        _currentState?.Enter();
    }

    public void Update()
    {
        _currentState?.Update();
    }
}