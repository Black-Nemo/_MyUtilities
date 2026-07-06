using UnityEngine;
using UnityEditor;

public class TimeControllerWindow : EditorWindow
{
    // Varsayılan zaman ölçeği
    float timeScale = 1.0f;
    
    // Pencereyi menüye ekleyen kod
    [MenuItem("Tools/Time Controller")]
    public static void ShowWindow()
    {
        GetWindow<TimeControllerWindow>("Time Controller");
    }

    void OnGUI()
    {
        GUILayout.Label("Oyun Hızı Kontrolü", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // Slider: 0.05 ile 10 arasında değer alır
        // Mevcut Time.timeScale değerini slider'a yansıtırız, böylece dışarıdan değişse bile slider güncel kalır.
        if (Application.isPlaying)
        {
            timeScale = Time.timeScale;
        }

        // Değer değiştiğinde algılamak için ChangeCheck bloğu kullanıyoruz
        EditorGUI.BeginChangeCheck();
        
        timeScale = EditorGUILayout.Slider("Hız (Time Scale)", timeScale, 0.05f, 10.0f);

        if (EditorGUI.EndChangeCheck())
        {
            // Eğer oyun oynatılıyorsa değeri uygula
            if (Application.isPlaying)
            {
                ApplyTimeScale(timeScale);
            }
        }

        GUILayout.Space(20);

        // Hızlı ayar butonları
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Durdur (0x)"))
        {
            timeScale = 0f;
            ApplyTimeScale(timeScale);
        }

        if (GUILayout.Button("Ağır Çekim (0.5x)"))
        {
            timeScale = 0.5f;
            ApplyTimeScale(timeScale);
        }

        if (GUILayout.Button("Normal (1x)"))
        {
            timeScale = 1.0f;
            ApplyTimeScale(timeScale);
        }

        if (GUILayout.Button("Hızlı (2x)"))
        {
            timeScale = 2.0f;
            ApplyTimeScale(timeScale);
        }

        GUILayout.EndHorizontal();
        
        // Bilgilendirme
        if (!Application.isPlaying)
        {
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Hız ayarları sadece Play Mode (Oyun Modu) etkinken çalışır.", MessageType.Info);
        }
    }

    // Time Scale'i ve fiziği senkronize bir şekilde değiştirir
    void ApplyTimeScale(float scale)
    {
        Time.timeScale = scale;
        
        // Fiziğin (Rigidbody) titrememesi için FixedDeltaTime'ı da orantılı değiştirmek gerekir.
        // Varsayılan FixedDeltaTime genellikle 0.02f'tir.
        Time.fixedDeltaTime = 0.02f * scale; 
    }
}