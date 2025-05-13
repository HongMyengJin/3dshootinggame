using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityMeshSimplifier;
public class LODDirectChildrenOnly : MonoBehaviour
{
    [MenuItem("Tools/LOD/Insert LODGroup to Children With Renderer")]
    static void InsertLODGroupToChildren()
    {
        GameObject root = Selection.activeGameObject;
        if (root == null)
        {
            Debug.LogWarning("오브젝트를 선택하세요.");
            return;
        }

        Transform parent = root.transform;
        int total = 0;
        int created = 0;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            // 조건: 자식의 자식이 1개뿐
            if (child.childCount != 1)
                continue;

            Transform inner = child.GetChild(0);
            MeshRenderer innerRenderer = inner.GetComponent<MeshRenderer>();
            if (innerRenderer == null)
                continue;

            total++;

            // 자식 오브젝트에 LODGroup 없으면 추가
            LODGeneratorHelper LODGeneratorHelper = child.GetComponent<LODGeneratorHelper>();
            if (LODGeneratorHelper == null)
                LODGeneratorHelper = child.gameObject.AddComponent<LODGeneratorHelper>();

            GenerateLODs(LODGeneratorHelper);
            created++;
        }

        Debug.Log($"총 {total}개의 자식 중 {created}개에 LODGroup을 설정했습니다.");
    }

    static private void GenerateLODs(LODGeneratorHelper lodGeneratorHelper)
    {
        EditorUtility.DisplayProgressBar("Generating LODs", "Generating LODs...", 0f);
        var lodGroup = LODGenerator.GenerateLODs(lodGeneratorHelper);
        if (lodGroup != null)
        {
            using (var serializedObject = new SerializedObject(lodGeneratorHelper))
            {
                serializedObject.UpdateIfRequiredOrScript();
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
            EditorUtility.SetDirty(lodGeneratorHelper.gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(lodGeneratorHelper.gameObject.scene);
            AssetDatabase.SaveAssets();
        }

        EditorUtility.ClearProgressBar();
    }
}