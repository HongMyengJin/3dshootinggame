using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
            LODGroup lodGroup = child.GetComponent<LODGroup>();
            if (lodGroup == null)
                lodGroup = child.gameObject.AddComponent<LODGroup>();

            // 자식 자신에도 렌더러가 있다면 포함
            List<Renderer> lod0Renderers = new List<Renderer> { innerRenderer };
            MeshRenderer selfRenderer = child.GetComponent<MeshRenderer>();
            if (selfRenderer != null)
                lod0Renderers.Add(selfRenderer);

            LOD[] lods = new LOD[]
            {
                new LOD(0.85f, lod0Renderers.ToArray()),
                new LOD(0.01f, new Renderer[0]) // Culled
            };

            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
            EditorUtility.SetDirty(lodGroup);
            created++;
        }

        Debug.Log($"✅ 총 {total}개의 자식 중 {created}개에 LODGroup을 설정했습니다.");
    }
}