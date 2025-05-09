using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    private List<IEnemy> _activeEnemies = new List<IEnemy>();
    public IReadOnlyList<IEnemy> GetActiveEnemies() => _activeEnemies;

    [System.Serializable]
    public struct EnemyType
    {
        public GameObject prefab;
        public int initialPoolSize;
        public float spawnInterval;
        public float spawnRange;
    }

    [SerializeField] private EnemyType[] enemyTypes;
    [SerializeField] private Transform[] spawnPoints;

    private Dictionary<GameObject, EnemyPool> pools = new Dictionary<GameObject, EnemyPool>();
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (EnemyType type in enemyTypes)
        {
            pools[type.prefab] = new EnemyPool(type.prefab, this.transform, type.initialPoolSize);
        }
    }

    private void OnEnable()
    {
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            EnemyType randomType = enemyTypes[Random.Range(0, enemyTypes.Length)];

            SpawnEnemy(randomType);

            yield return new WaitForSeconds(randomType.spawnInterval);
        }
    }

    private void SpawnEnemy(EnemyType enemyType)
    {
        if (pools.Count == 0)
            return;

        IEnemy enemy = pools[enemyType.prefab].Get();

        enemy.Initialize(PickSpawnPos(enemyType.spawnRange));
        enemy.OnSpawn();
        _activeEnemies.Add(enemy); // 추가
    }

    // 적이 죽을 때
    public void OnEnemyDead(IEnemy enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    private Vector3 PickSpawnPos(float spawnRange)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return Vector3.zero;
        }

        Transform basePoint = PickBase();
        return RandomAround(basePoint.position, spawnRange);
    }

    private Transform PickBase()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    private Vector3 RandomAround(Vector3 center, float range)
    {
        Vector2 offset = Random.insideUnitCircle * range;
        return new Vector3(center.x + offset.x, center.y, center.z + offset.y);
    }



}
