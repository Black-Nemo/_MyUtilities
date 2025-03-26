using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class VersionPrompt : IPreprocessBuildWithReport
{
    // Build adımının sırasını belirleyin, 0 ilk çalışacağı anlamına gelir
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Versiyon numarası isteyen bir pencere açın
        string newVersion = EditorUtility.DisplayDialogComplex("Versiyon Numarası", "Versiyon numarasını girin:", "Tamam", "İptal", null).ToString();

        // Kullanıcı iptal ettiyse işlemi durdurun
        if (string.IsNullOrEmpty(newVersion))
        {
            Debug.LogError("Build, kullanıcı tarafından iptal edildi.");
            throw new BuildFailedException("Build, kullanıcı tarafından iptal edildi.");
        }

        // Yeni versiyonu ayarlayın
        PlayerSettings.bundleVersion = newVersion;

        // Yeni versiyonu onaylamak için log yazdırın
        Debug.Log("Versiyon şu şekilde ayarlandı: " + newVersion);
    }
}
