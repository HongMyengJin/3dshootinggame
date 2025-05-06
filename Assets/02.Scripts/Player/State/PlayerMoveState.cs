using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private IPlayerMoveContext _context;
    public PlayerMoveState(IPlayerMoveContext context) : base() 
    {
        _context = context;
    }

    public override void Update()
    {
        moveStrategy.Move(_context);
    }
}
