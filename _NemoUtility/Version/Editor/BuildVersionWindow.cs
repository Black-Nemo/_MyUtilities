using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace NemoUtility
{
    public class BuildVersionWindow : EditorWindow
    {
        private static string versionInput = "";
        private static bool confirmed = false;
        private static bool canceled = false;

        public static void ShowWindow(string currentVersion)
        {
            versionInput = currentVersion;
            confirmed = false;
            canceled = false;

            BuildVersionWindow window = GetWindow<BuildVersionWindow>(true, "Build Version", true);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 300, 100);
            window.ShowModal(); // Modal window to block build until input is given
        }

        void OnGUI()
        {
            GUILayout.Label("Enter new build version:", EditorStyles.boldLabel);
            versionInput = EditorGUILayout.TextField("Version", versionInput);

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                canceled = true;
                Close();
            }

            if (GUILayout.Button("Confirm"))
            {
                confirmed = true;
                Close();
            }
            GUILayout.EndHorizontal();
        }

        public static bool TryGetVersion(out string version)
        {
            version = versionInput;
            return confirmed && !string.IsNullOrEmpty(versionInput);
        }

        public static bool WasCanceled() => canceled;
    }
}