using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private Enemy enemy;
    public void Enter(Enemy e) => enemy = e;
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
