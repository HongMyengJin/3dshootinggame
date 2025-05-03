using System.Collections.Generic;

public class PlayerStateMachine
{
    private Dictionary<PlayerStateType, PlayerState> _stateMap;
    private IPlayerState _currentState;

    public PlayerStateMachine(PlayerController player)
    {
        _stateMap = new Dictionary<PlayerStateType, PlayerState>
        {
            { PlayerStateType.Walk, new PlayerMoveState(player) }
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