using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

public class BuildToolWindow : EditorWindow
{
    // Checkbox durumlarını tutacak değişkenler
    bool buildWindows = true;
    bool buildLinuxServer = true;
    bool buildWebGL = false;
    bool buildMobile = false; // Android varsayımı

    string version;

    [MenuItem("Build/Build Manager Window")]
    public static void ShowWindow()
    {
        GetWindow<BuildToolWindow>("Build Yöneticisi");
    }

    private void OnEnable()
    {
        version = PlayerSettings.bundleVersion;
    }

    private void OnGUI()
    {
        // ===== VERSION =====
        GUILayout.Label("Build Version", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        version = EditorGUILayout.TextField("Version", version);
        if (EditorGUI.EndChangeCheck())
        {
            PlayerSettings.bundleVersion = version;
        }

        EditorGUILayout.Space(15);

        // ===== PLATFORMS =====
        GUILayout.Label("Platform Seçimi", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        buildWindows = EditorGUILayout.Toggle("Windows Client", buildWindows);
        buildLinuxServer = EditorGUILayout.Toggle("Linux Server", buildLinuxServer);
        buildWebGL = EditorGUILayout.Toggle("WebGL", buildWebGL);
        buildMobile = EditorGUILayout.Toggle("Mobile (Android)", buildMobile);

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Seçilen platformlar sırasıyla 'Builds' klasörüne çıkarılacaktır.", MessageType.Info);
        EditorGUILayout.Space();

        if (GUILayout.Button("SEÇİLENLERİ BUILD AL", GUILayout.Height(40)))
        {
            RunBuildProcess();
        }
    }

    private void RunBuildProcess()
    {
        // güvenlik: boş version ile build alma
        if (string.IsNullOrWhiteSpace(version))
        {
            EditorUtility.DisplayDialog("Hata", "Version boş olamaz.", "Tamam");
            return;
        }

        // Mevcut aktif platform ve subtarget ayarlarını kaydet
        BuildTarget originalTarget = EditorUserBuildSettings.activeBuildTarget;
        BuildTargetGroup originalGroup = BuildPipeline.GetBuildTargetGroup(originalTarget);
        StandaloneBuildSubtarget originalSubtarget = EditorUserBuildSettings.standaloneBuildSubtarget;

        PlayerSettings.bundleVersion = version;

        if (buildLinuxServer)
        {
            Debug.Log(">>> Linux Server Build Başlıyor...");
            BuildPlatform(BuildTarget.StandaloneLinux64, StandaloneBuildSubtarget.Server, $"{Application.productName}_LinuxServer/{Application.productName}.x86_64");
        }

        if (buildWindows)
        {
            Debug.Log(">>> Windows Client Build Başlıyor...");
            BuildPlatform(BuildTarget.StandaloneWindows64, StandaloneBuildSubtarget.Player, $"{Application.productName}_Windows/{Application.productName}.exe");
        }

        if (buildWebGL)
        {
            Debug.Log(">>> WebGL Build Başlıyor...");
            BuildPlatform(BuildTarget.WebGL, StandaloneBuildSubtarget.Player, $"{Application.productName}_WebGL");
        }

        if (buildMobile)
        {
            Debug.Log(">>> Android Build Başlıyor...");
            BuildPlatform(BuildTarget.Android, StandaloneBuildSubtarget.Player, $"{Application.productName}_Android/{Application.productName}.apk");
        }

        Debug.Log("--- Tüm İşlemler Tamamlandı ---");

        // İşlem bitince Unity'yi orijinal ayarlarına geri döndür
        if (EditorUserBuildSettings.activeBuildTarget != originalTarget)
        {
            Debug.Log($">>> Orijinal platforma ({originalTarget}) geri dönülüyor...");
            EditorUserBuildSettings.SwitchActiveBuildTarget(originalGroup, originalTarget);
        }

        if (EditorUserBuildSettings.standaloneBuildSubtarget != originalSubtarget)
        {
            EditorUserBuildSettings.standaloneBuildSubtarget = originalSubtarget;
        }
    }

    private void BuildPlatform(BuildTarget target, StandaloneBuildSubtarget subtarget, string relativePath)
    {
        // 1. ADIM: Aktif platformu değiştir (Hataları önlemek için en kritik adım)
        if (EditorUserBuildSettings.activeBuildTarget != target)
        {
            Debug.Log($">>> Platform {target} olarak değiştiriliyor...");
            // Bu işlem assetleri o platform için re-import eder
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(target), target);
        }

        string fullPath = Path.Combine("Builds", relativePath);
        string folderPath = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(folderPath))
            Directory.CreateDirectory(folderPath);

        var buildOptions = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = fullPath,
            target = target,
            options = BuildOptions.None
        };

        // Server subtarget ayarı
        if (target == BuildTarget.StandaloneWindows64 || target == BuildTarget.StandaloneLinux64)
        {
            EditorUserBuildSettings.standaloneBuildSubtarget = subtarget;
        }

        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        // ... geri kalan loglama işlemleri
    }

    private static string[] GetScenes()
    {
        var enabledScenes = new System.Collections.Generic.List<string>();

        foreach (var s in EditorBuildSettings.scenes)
            if (s.enabled)
                enabledScenes.Add(s.path);

        return enabledScenes.ToArray();
    }
}
