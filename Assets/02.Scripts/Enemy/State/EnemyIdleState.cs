using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyIdleState : IEnemyState
{
    private IEnemyContext context;
    public void Enter(IEnemyContext ctx)
    {
        context = ctx;

        context.ScheduleStateChange();
    }
    public void Update()
    {
        // 행동: 가만히 있는다.
        if (Vector3.Distance(transform.position, _player.transform.position) < _stat.FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }

        StartCoroutine(Patrol_Coroutine());
    }
    public void Exit()
    {

    }


}
