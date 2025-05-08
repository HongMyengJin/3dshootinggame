using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : PlayerState
{
    private IPlayerDamageContext _context;
    public PlayerDamagedState(IPlayerDamageContext context) : base() 
    {
        _context = context;
    }

    public override void Enter()
    {
        _context.Animator.SetTrigger("Damage");
        
        _context.ScheduleStateChange(PlayerStateType.Walk, 1.0f);
    }
    public override void Update()
    {
    }
}
