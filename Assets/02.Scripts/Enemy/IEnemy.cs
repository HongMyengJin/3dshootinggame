using UnityEngine;

public interface IEnemy
{
    EnemyStateType CurrentType { get; }
    Transform Transform { get; }
    bool IsActive { get; }
    void Initialize(Vector3 spawnPosition);
    void OnSpawn();
    void OnDespawn();
}
