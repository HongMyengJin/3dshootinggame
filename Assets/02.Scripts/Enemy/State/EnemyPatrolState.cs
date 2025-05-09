﻿using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IEnemyState
{
    private IEnemyPatrolContext context;
    private IEnemyStrategy<IEnemyPatrolContext> patrolStrategy;
    public EnemyPatrolState(IEnemyStrategy<IEnemyPatrolContext> patrolStrategy)
    {
        this.patrolStrategy = patrolStrategy;
    }
    public void Enter(IEnemyContext ctx)
    {
        context = ctx as IEnemyPatrolContext;
        patrolStrategy.Execute(context);
    }
    public void Update()
    {
        if (context == null)
            return;

        patrolStrategy.Update(context);

        // 상태 전환 판단 - State 관리

        if (Vector3.Distance(context.Self.position, context.Target.position) < context.State.FindDistance)
        {
            context.ScheduleStateChange(EnemyStateType.Chase);
            return;
        }
    }
    public void Exit()
    {
        if (context == null || context.Agent == null)
            return;

        patrolStrategy.Exit(context);
    }
}

