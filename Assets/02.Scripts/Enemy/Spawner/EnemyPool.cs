using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{
    private GameObject prefab;
    private Transform parent;
    private List<IEnemy> pool = new List<IEnemy>();

    public EnemyPool(GameObject prefab, Transform parent, int initialSize)
    {
        this.prefab = prefab;
        this.parent = parent;
        Initialize(initialSize);
    }

    private void Initialize(int size)
    {
        for (int i = 0; i < size; i++)
        {
            Create();
        }
    }

    private IEnemy Create()
    {
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.SetActive(false);
        var enemy = obj.GetComponent<IEnemy>();
        pool.Add(enemy);
        return enemy;
    }

    public IEnemy Get()
    {
        foreach (var enemy in pool)
        {
            if (!enemy.IsActive)
            {
                return enemy;
            }
        }

        return Create();
    }
}