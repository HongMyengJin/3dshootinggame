using UnityEngine;

public interface IEnemyContext
{
    Transform Self { get; } // �� �ڽ� ��ġ
    Transform Target { get; } // Ÿ�� ��ġ
    float AttackDistance { get; } // ���� �Ÿ�
    float Speed { get; } // �ӵ�

    void ChangeState(EnemyStateType nextState);
    void TriggerAttack();
}
