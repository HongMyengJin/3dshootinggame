using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityMeshSimplifier;
public class LODGroupRewriter : MonoBehaviour
{
    [MenuItem("Tools/LOD/Reset LODGroup Ratios(90/80/2%)")]
    static void ResetAllLODRatios()
    {
        GameObject root = Selection.activeGameObject;
        if (root == null)
        {
            Debug.LogWarning("루트 오브젝트를 선택하세요.");
            return;
        }

        var lodGroups = root.GetComponentsInChildren<LODGroup>(true);
        int changed = 0;

        foreach (LODGroup group in lodGroups)
        {
            var lods = group.GetLODs();
            if (lods.Length < 3)
            {
                Debug.LogWarning($"{group.name}: LOD 단계가 부족하여 건너뜀");
                continue;
            }

            // 비율 재설정: 0.9, 0.8, 0.02, 0 (cull)
            lods[0].screenRelativeTransitionHeight = 0.9f;
            lods[1].screenRelativeTransitionHeight = 0.8f;
            lods[2].screenRelativeTransitionHeight = 0.02f;

            // 나머지 (cull)
            for (int i = 3; i < lods.Length; i++)
                lods[i].screenRelativeTransitionHeight = 0.0f;

            group.SetLODs(lods);
            group.RecalculateBounds();

            EditorUtility.SetDirty(group);
            changed++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"총 {changed}개의 LODGroup에 비율을 적용했습니다.");
    }
}