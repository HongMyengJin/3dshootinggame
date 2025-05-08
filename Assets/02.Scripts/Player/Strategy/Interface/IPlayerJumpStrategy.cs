using System;
using UnityEngine;

public interface IPlayerJumpStrategy
{
    event Action OnJumpPerformed;
    void Jump(IPlayerJumpContext context);
}
