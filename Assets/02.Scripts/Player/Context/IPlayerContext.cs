using UnityEngine;

public interface IPlayerContext 
{
    Transform Transform { get; }

    void ScheduleStateChange(PlayerStateType next, float delay = 0.0f);
}
