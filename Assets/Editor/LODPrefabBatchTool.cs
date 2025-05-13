using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityMeshSimplifier;
public class LODPrefabBatchTool : MonoBehaviour
{
    [MenuItem("Tools/LOD/Insert LODGroup to Prefabs In Folder (Root Only)")]
    static void InsertLODGroupToPrefabsRootOnly()
    {
        string folderPath = "Assets/LeartesStudios/LostCave/HDRP/Art/Prefabs";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        int total = 0;
        int updated = 0;

        AssetDatabase.StartAssetEditing(); // ✅ 최적화 시작
        try
        {
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null) continue;

                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                if (instance == null) continue;

                if (instance.GetComponentInChildren<LODGeneratorHelper>() != null)
                {
                    // 이미 LODGeneratorHelper가 있음 → 건너뜀
                    Debug.Log($"이미 LODGeneratorHelper 있음: {path}");
                    GameObject.DestroyImmediate(instance);
                    continue;
                }

                // 루트 또는 첫 자식에 붙이기
                LODGeneratorHelper helper = instance.GetComponent<LODGeneratorHelper>();
                if (helper == null)
                {
                    if (instance.transform.childCount > 0)
                    {
                        if (instance.transform.GetChild(0).gameObject.GetComponent<LODGeneratorHelper>() == null)
                        {
                            helper = instance.transform.GetChild(0).gameObject.AddComponent<LODGeneratorHelper>();
                            Debug.Log($"첫 자식에 LODGeneratorHelper 추가: {path}");
                        }
                        else
                        {
                            Debug.Log($"해당 자식에 이미 존재: {path}");
                        }
                        
                    }
                    else
                    {
                        Debug.LogWarning($"자식 없음, LODGeneratorHelper 추가 못함: {path}");
                        GameObject.DestroyImmediate(instance);
                        continue;
                    }
                }

                bool success = GenerateLODs(helper);

                if (success)
                {
                    PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.AutomatedAction);
                    updated++;
                }

                GameObject.DestroyImmediate(instance);
                total++;
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing(); // ✅ 최적화 종료
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }

        Debug.Log($"총 {updated}/{total}개의 프리팹에 LODGroup 적용 완료");
    }

    static private bool GenerateLODs(LODGeneratorHelper lodGeneratorHelper)
    {
        EditorUtility.DisplayProgressBar("Generating LODs", lodGeneratorHelper.name, 0f);

        var lodGroup = LODGenerator.GenerateLODs(lodGeneratorHelper);
        if (lodGroup != null)
        {
            using (var serializedObject = new SerializedObject(lodGeneratorHelper))
            {
                serializedObject.UpdateIfRequiredOrScript();
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorUtility.SetDirty(lodGeneratorHelper.gameObject);
            return true;
        }

        return false;
    }
}