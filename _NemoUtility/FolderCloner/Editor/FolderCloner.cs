using UnityEngine;
using UnityEditor;
using System.IO;

public class FolderCloner : EditorWindow
{
    private DefaultAsset targetFolder;
    private string newName = "";

    [MenuItem("Tools/Klasör Klonla ve Yeniden Adlandır")]
    public static void ShowWindow()
    {
        GetWindow<FolderCloner>("Klonlayıcı");
    }

    private void OnGUI()
    {
        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Klasör Seç", targetFolder, typeof(DefaultAsset), false);
        newName = EditorGUILayout.TextField("Yeni Ad", newName);

        if (GUILayout.Button("Uygula") && targetFolder != null && !string.IsNullOrEmpty(newName))
        {
            string sourcePath = AssetDatabase.GetAssetPath(targetFolder);
            if (!AssetDatabase.IsValidFolder(sourcePath)) return;

            string parentPath = Path.GetDirectoryName(sourcePath);
            string newFolderPath = AssetDatabase.GenerateUniqueAssetPath(parentPath + "/" + newName);

            AssetDatabase.CopyAsset(sourcePath, newFolderPath);
            AssetDatabase.Refresh();

            string[] guids = AssetDatabase.FindAssets("", new[] { newFolderPath });
            int index = 0;

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (AssetDatabase.IsValidFolder(assetPath)) continue;

                string uniqueName = guids.Length > 1 ? $"{newName}" : newName;
                AssetDatabase.RenameAsset(assetPath, uniqueName);

                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                if (so != null)
                {
                    SerializedObject serializedObj = new SerializedObject(so);
                    SerializedProperty nameProp = serializedObj.FindProperty("Name");
                    
                    if (nameProp == null) nameProp = serializedObj.FindProperty("name");

                    if (nameProp != null && nameProp.propertyType == SerializedPropertyType.String)
                    {
                        nameProp.stringValue = newName;
                        serializedObj.ApplyModifiedProperties();
                        EditorUtility.SetDirty(so);
                    }
                }
                index++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}