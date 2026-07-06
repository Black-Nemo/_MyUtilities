using UnityEngine;
using UnityEditor;

public class FPSLimiterTool : EditorWindow
{
    // Varsayılan değer
    private int targetFPS = 60;
    
    // Pencereyi menüden açmak için
    [MenuItem("Tools/FPS Limiter")]
    public static void ShowWindow()
    {
        GetWindow<FPSLimiterTool>("FPS Limiter");
    }

    private void OnGUI()
    {
        GUILayout.Label("FPS Kontrol Paneli", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // --- VSync Kontrolü ---
        // FPS limitinin çalışması için VSync'in KAPALI olması gerekir (Count = 0).
        if (QualitySettings.vSyncCount != 0)
        {
            EditorGUILayout.HelpBox("VSync (Dikey Senkronizasyon) şu an AÇIK.\nFPS limiti belirleyebilmek için VSync kapalı olmalıdır.", MessageType.Warning);
            
            if (GUILayout.Button("VSync'i Kapat ve Devam Et"))
            {
                QualitySettings.vSyncCount = 0;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("VSync Kapalı. Manuel FPS kontrolü aktif.", MessageType.Info);
        }

        GUILayout.Space(10);

        // --- Slider Bölümü ---
        // 1 ile 1000 arasında bir slider
        targetFPS = EditorGUILayout.IntSlider("Hedef FPS", targetFPS, 1, 1000);

        GUILayout.Space(10);

        // --- Uygulama Butonları ---
        
        // Değer değiştiğinde otomatik uygula (İsteğe bağlı, sliderı oynattığın an değişir)
        if (GUI.changed) 
        {
            ApplyFPS();
        }

        // Hızlı ayarlar için butonlar
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("30 FPS")) { targetFPS = 30; ApplyFPS(); }
        if (GUILayout.Button("60 FPS")) { targetFPS = 60; ApplyFPS(); }
        if (GUILayout.Button("120 FPS")) { targetFPS = 120; ApplyFPS(); }
        if (GUILayout.Button("Sınırsız (-1)")) { targetFPS = -1; ApplyFPS(); }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        
        // Şu anki durumu göster
        GUILayout.Label($"Aktif Application.targetFrameRate: {Application.targetFrameRate}", EditorStyles.miniLabel);
    }

    private void ApplyFPS()
    {
        // Eğer -1 seçilirse Unity limiti kaldırır
        Application.targetFrameRate = targetFPS;
    }
}