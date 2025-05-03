using UnityEngine;

public class PlayerJumpStrategy : IPlayerJumpStrategy
{
    public void Jump(IPlayerJumpContext context)
    {
        if (context is PlayerController player)
        {
            player.SetVerticalVelocity(player.JumpForce);
        }
    }
}
