using UnityEngine;
using UnityEditor;
using System.IO;

public class URPMaterialStellarGirlConverter : EditorWindow
{
    private static readonly string folderPath = "Assets/10.Assets/Stellar Girl Celeste";

    [MenuItem("Tools/URP Material Converter (Stellar Girl)")]
    static void Init()
    {
        URPMaterialStellarGirlConverter window = (URPMaterialStellarGirlConverter)EditorWindow.GetWindow(typeof(URPMaterialStellarGirlConverter));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label($"변환 대상 폴더: {folderPath}", EditorStyles.boldLabel);

        if (GUILayout.Button("Convert Materials in Stellar Girl Folder"))
        {
            if (Directory.Exists(folderPath))
            {
                ConvertMaterialsInFolder(folderPath);
            }
            else
            {
                Debug.LogError("폴더를 찾을 수 없습니다: " + folderPath);
            }
        }
    }

    private void ConvertMaterialsInFolder(string path)
    {
        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { path });
        int converted = 0;

        foreach (string guid in guids)
        {
            string matPath = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);

            if (mat != null && mat.shader.name.Contains("Standard"))
            {
                Color albedoColor = mat.HasProperty("_Color") ? mat.GetColor("_Color") : Color.white;
                Texture albedoTex = mat.HasProperty("_MainTex") ? mat.GetTexture("_MainTex") : null;
                float metallic = mat.HasProperty("_Metallic") ? mat.GetFloat("_Metallic") : 0f;
                float smoothness = mat.HasProperty("_Glossiness") ? mat.GetFloat("_Glossiness") : 0f;

                mat.shader = Shader.Find("Universal Render Pipeline/Lit");

                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", albedoColor);

                if (mat.HasProperty("_BaseMap"))
                    mat.SetTexture("_BaseMap", albedoTex);

                if (mat.HasProperty("_Metallic"))
                    mat.SetFloat("_Metallic", metallic);

                if (mat.HasProperty("_Smoothness"))
                    mat.SetFloat("_Smoothness", smoothness);

                converted++;
            }
        }

        Debug.Log($"변환 완료: {converted}개 메테리얼을 URP/Lit로 변경했습니다. (폴더: {folderPath})");
    }
}
