using UnityEngine;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine.U2D;
using System.Collections.Generic;
using System.Linq;

namespace NemoUtility
{
    public class UnatlasedTextureFinder : EditorWindow
    {
        private List<SpriteAtlas> targetAtlases = new List<SpriteAtlas>();
        private Vector2 scrollPos;
        private List<Texture2D> missingTextures = new List<Texture2D>();
        private bool isScanning = false;
        private int selectedAtlasIndex = 0;

        [MenuItem("Tools/Atlas Eksiklerini Bul")]
        public static void ShowWindow()
        {
            GetWindow<UnatlasedTextureFinder>("Atlas Checker");
        }

        void OnGUI()
        {
            GUILayout.Label("Build'de Kullanılan Ama Atlaslarda Olmayan Dokular", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // --- Atlas Listesi Bölümü ---
            GUILayout.Label("Kontrol Edilecek Atlaslar:", EditorStyles.miniBoldLabel);
            for (int i = 0; i < targetAtlases.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                targetAtlases[i] = (SpriteAtlas)EditorGUILayout.ObjectField($"Atlas {i + 1}", targetAtlases[i], typeof(SpriteAtlas), false);
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    targetAtlases.RemoveAt(i);
                    i--;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+ Yeni Atlas Ekle"))
            {
                targetAtlases.Add(null);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();

            if (GUILayout.Button("Taramayı Başlat (Tüm Atlaslar İçin)", GUILayout.Height(30)))
            {
                FindUnatlasedTextures();
            }

            if (isScanning)
            {
                EditorGUILayout.HelpBox("Taranıyor...", MessageType.Info);
            }

            if (missingTextures.Count > 0)
            {
                EditorGUILayout.Space();
                GUILayout.Label($"Eksik Dokular ({missingTextures.Count}):", EditorStyles.boldLabel);

                // Hangi atlasın hedef olacağını seçme
                if (targetAtlases.Count > 0)
                {
                    var validAtlases = targetAtlases.Where(a => a != null).ToList();
                    if (validAtlases.Count > 0)
                    {
                        string[] atlasNames = validAtlases.Select(a => a.name).ToArray();
                        selectedAtlasIndex = EditorGUILayout.Popup("Eklenecek Hedef Atlas:", selectedAtlasIndex, atlasNames);

                        if (selectedAtlasIndex >= validAtlases.Count) selectedAtlasIndex = 0;

                        scrollPos = GUILayout.BeginScrollView(scrollPos);
                        foreach (var tex in missingTextures.ToList())
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.ObjectField(tex, typeof(Texture2D), false);
                            if (GUILayout.Button("Atlasa Ekle", GUILayout.Width(120)))
                            {
                                AddTextureToAtlas(tex, validAtlases[selectedAtlasIndex]);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        GUILayout.EndScrollView();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Dokuları eklemek için listeye en az bir geçerli atlas ekleyin.", MessageType.Warning);
                    }
                }
            }
            else if (missingTextures.Count == 0 && targetAtlases.Any(a => a != null) && !isScanning)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Tebrikler! Tüm dokular seçili atlaslarda mevcut.", MessageType.Info);
            }
        }

        void FindUnatlasedTextures()
        {
            if (targetAtlases.Count == 0 || targetAtlases.All(a => a == null))
            {
                EditorUtility.DisplayDialog("Hata", "Lütfen en az bir geçerli Sprite Atlas ekleyin.", "Tamam");
                return;
            }

            isScanning = true;
            missingTextures.Clear();

            // 1. Build Sahneleri
            var activeScenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => AssetDatabase.LoadAssetAtPath<SceneAsset>(s.path))
                .ToArray();

            // 2. Bağımlılıklar
            var allDependencies = EditorUtility.CollectDependencies(activeScenes);
            var buildTextures = allDependencies.OfType<Texture2D>().ToList();

            // 3. TÜM Atlaslardaki dokuları topla
            HashSet<Texture2D> allAtlasedTextures = new HashSet<Texture2D>();
            foreach (var atlas in targetAtlases)
            {
                if (atlas == null) continue;

                Object[] packables = atlas.GetPackables();
                foreach (var packable in packables)
                {
                    if (packable is DefaultAsset)
                    {
                        string folderPath = AssetDatabase.GetAssetPath(packable);
                        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath });
                        foreach (var guid in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(guid);
                            allAtlasedTextures.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(path));
                        }
                    }
                    else if (packable is Texture2D tex) allAtlasedTextures.Add(tex);
                    else if (packable is Sprite sprite) allAtlasedTextures.Add(sprite.texture);
                }
            }

            // 4. Karşılaştır
            foreach (var tex in buildTextures)
            {
                string path = AssetDatabase.GetAssetPath(tex);
                if (string.IsNullOrEmpty(path) || path.StartsWith("Packages/") || path.StartsWith("Library/") || path.Contains("Editor"))
                    continue;

                if (!allAtlasedTextures.Contains(tex) && !missingTextures.Contains(tex))
                {
                    missingTextures.Add(tex);
                }
            }

            isScanning = false;
        }

        void AddTextureToAtlas(Texture2D tex, SpriteAtlas atlas)
        {
            if (atlas == null) return;

            Object[] currentPackables = atlas.GetPackables();
            List<Object> newPackables = new List<Object>(currentPackables) { tex };

            atlas.Remove(currentPackables);
            atlas.Add(newPackables.ToArray());

            SpriteAtlasUtility.PackAtlases(new[] { atlas }, EditorUserBuildSettings.activeBuildTarget);
            missingTextures.Remove(tex);
        }
    }
}
