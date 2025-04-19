using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace NemoUtility
{
    public class BuildVersionProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            string currentVersion = PlayerSettings.bundleVersion;
            BuildVersionWindow.ShowWindow(currentVersion);

            if (BuildVersionWindow.WasCanceled())
            {
                throw new BuildFailedException("Build was canceled by the user.");
            }

            if (BuildVersionWindow.TryGetVersion(out string newVersion))
            {
                PlayerSettings.bundleVersion = newVersion;
                UnityEngine.Debug.Log($"Build version updated to: {newVersion}");
            }
        }
    }

}

