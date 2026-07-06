using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace NemoUtility
{

    public class MaterialFinder
    {
        [MenuItem("Tools/Mage Fight/Select Editable Lit Materials")]
        public static void SelectLitMaterials()
        {
            string[] allMaterials = AssetDatabase.FindAssets("t:Material");
            List<Object> foundMaterials = new List<Object>();
            int skippedCount = 0;

            foreach (string guid in allMaterials)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

                if (mat == null) continue;

                // 1. Sadece "Universal Render Pipeline/Lit" kullananları filtrele
                if (mat.shader.name == "Universal Render Pipeline/Lit")
                {
                    // 2. KONTROL: Eğer materyal "Packages" içindeyse düzenlenemez, ATLA.
                    if (!path.StartsWith("Assets/"))
                    {
                        skippedCount++;
                        continue;
                    }

                    // 3. KONTROL: Eğer materyal bir FBX'in içine gömülüyse (SubAsset), ATLA.
                    if (AssetDatabase.IsSubAsset(mat))
                    {
                        skippedCount++;
                        continue;
                    }

                    foundMaterials.Add(mat);
                }
            }

            if (foundMaterials.Count > 0)
            {
                Selection.objects = foundMaterials.ToArray();
                Debug.Log($"<color=green>{foundMaterials.Count}</color> tane düzenlenebilir materyal seçildi. " +
                          $"<color=yellow>{skippedCount}</color> tane kilitli (FBX içinde veya Package) materyal atlandı.");
            }
            else
            {
                Debug.LogWarning("Düzenlenebilir 'Lit' materyal bulunamadı. " +
                                 "Atlanan kilitli materyal sayısı: " + skippedCount);
            }
        }
    }
}
