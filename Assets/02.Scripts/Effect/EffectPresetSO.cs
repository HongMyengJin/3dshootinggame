using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectPresetSO", menuName = "Scriptable Objects/EffectPresetSO")]
public class EffectPresetSO : ScriptableObject
{
    [System.Serializable]
    public struct EffectInfo
    {
        public EffectType type;
        public GameObject prefab;
        public int preloadCount;
    }

    [Tooltip("적용 대상 오브젝트")]
    public ObjectType Target;
    [Tooltip("이펙트 프리팹 목록")]
    public List<EffectInfo> Effects;

    public GameObject GetPrefab(EffectType type)
    {
        foreach (EffectInfo info in Effects)
        {
            if (info.type == type)
                return info.prefab;
        }
        return null;
    }
}
