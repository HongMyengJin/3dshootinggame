using UnityEngine;

public interface IEnemy
{
    bool IsActive { get; }
    void Initialize(Vector3 spawnPosition);
    void OnSpawn();
    void OnDespawn();
}
