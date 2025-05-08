using System;
using UnityEngine;

public class PlayerJumpStrategy : IPlayerJumpStrategy
{
    public event Action OnJumpPerformed;
    public void Jump(IPlayerJumpContext context)
    {
        if (context is PlayerController player)
        {
            player.SetVerticalVelocity(player.JumpForce);
            OnJumpPerformed?.Invoke();
        }
    }
}
