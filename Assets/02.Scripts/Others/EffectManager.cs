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


    [SerializeField] private List<EffectPresetSO> _presets;
    private readonly Dictionary<ObjectType, EffectPresetSO> _presetMap = new();
    private readonly Dictionary<GameObject, Queue<GameObject>> _pool = new();

    private void PreloadEffect()
    {
        foreach (EffectPresetSO preset in _presets)
        {
            _presetMap[preset.Target] = preset;

            foreach (EffectPresetSO.EffectInfo pair in preset.Effects)
            {
                if (!_pool.ContainsKey(pair.prefab))
                    _pool[pair.prefab] = new Queue<GameObject>();

                for (int i = 0; i < pair.preloadCount; i++)
                {
                    GameObject obj = Instantiate(pair.prefab);
                    obj.SetActive(false);
                    _pool[pair.prefab].Enqueue(obj);
                }
            }
        }
    }

    public void Play(ObjectType target, EffectType type, Vector3 position, Quaternion rotation, float playTime = 1.0f, float waitTime = 0.0f)
    {
        if (!_presetMap.TryGetValue(target, out var preset)) 
            return;

        GameObject prefab = preset.GetPrefab(type);
        if (prefab == null) 
            return;

        if (!_pool.TryGetValue(prefab, out var queue))
        {
            queue = new Queue<GameObject>();
            _pool[prefab] = queue;
        }

        GameObject obj = queue.Count > 0 ? queue.Dequeue() : Instantiate(prefab);
        obj.transform.SetPositionAndRotation(position, rotation);

        StartCoroutine(ReleaseAfter(prefab, obj, playTime, waitTime));
    }

    private IEnumerator ReleaseAfter(GameObject prefab, GameObject obj, float delay, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        obj.SetActive(true);

        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
        _pool[prefab].Enqueue(obj);
    }
}
