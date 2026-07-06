using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NemoUtility
{
    [CustomEditor(typeof(Localization))]
    public class LocalizationEditor : Editor
    {
        private Localization _myData;

        private string _csvFilePath = "";
        public override void OnInspectorGUI()
        {
            // Varsayılan inspector
            DrawDefaultInspector();

            _myData = (Localization)target;

            GUILayout.Space(10);
            if (GUILayout.Button("ImportCsv"))
            {
                _myData.LocalizeStrings = GetLocalizeStringTable();
                EditorUtility.SetDirty(_myData);
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("Edit"))
            {
                ChangeValues();
            }
        }

        private List<LocalizeString> GetLocalizeStringTable()
        {
            List<LocalizeString> localizeStrings = new List<LocalizeString>();

            TextAsset csvFile = _myData.CsvFile;
            if (csvFile == null)
            {
                Debug.LogError("CSV File is not assigned on the Localization asset!");
                return localizeStrings;
            }

            string[] lines = csvFile.text.Split('\n');
            Debug.Log("l" + lines.Length);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                localizeStrings.Add(GetLocalizeString(line.Trim()));
            }
            return localizeStrings;
        }
        private LocalizeString GetLocalizeString(string s)
        {
            string[] values = s.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Replace("\"", "");
            }

            Debug.Log("v" + values.Length);

            LocalizeString localizeString = new LocalizeString();
            localizeString.Key = values[0];

            localizeString.LocalizedStringTableSlots = new List<LocalizedStringTableSlot>();
            for (int i = 1; i < values.Length; i++)
            {
                string value = values[i];
                localizeString.LocalizedStringTableSlots.Add(new LocalizedStringTableSlot()
                {
                    Localize = _myData.Localizes[i - 1],
                    LocalizeString = value
                });
            }
            return localizeString;
        }

        private void ChangeValues()
        {
            _myData.EditLines();
            EditorUtility.SetDirty(_myData);
            AssetDatabase.SaveAssets();
        }
    }
}