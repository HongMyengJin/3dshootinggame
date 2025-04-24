using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PreloadEffect();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class EffectInfo
    {
        public EffectType effectType;
        public GameObject prefab;
        public int preloadCount = 5;
    }


    [SerializeField] private List<EffectInfo> effects;
    private readonly Dictionary<EffectType, Queue<GameObject>> effectPool = new Dictionary<EffectType, Queue<GameObject>>();


    private void PreloadEffect()
    {
        foreach (var effect in effects)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < effect.preloadCount; i++)
            {
                GameObject obj = Instantiate(effect.prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            effectPool.Add(effect.effectType, pool);
        }
    }

    public void Play(EffectType effectType, Vector3 position, Quaternion rotation = default)
    {
        if (!effectPool.TryGetValue(effectType, out var queue)) // 이펙트x
            return;

        GameObject obj = queue.Count > 0 ? queue.Dequeue() : Instantiate(GetPrefab(effectType));

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        StartCoroutine(ReleaseAfter(effectType, obj, 2f)); // 2초 후에 비활성화
    }

    GameObject GetPrefab(EffectType effectType)
    {
        return effects.Find(effect => effect.effectType == effectType)?.prefab;
    }

    IEnumerator ReleaseAfter(EffectType effectType, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        effectPool[effectType].Enqueue(obj);
    }
}
