using UnityEngine;

public interface IEnemyState
{
    void Enter(IEnemyContext ctx);
    void Update();
    void Exit();
}
