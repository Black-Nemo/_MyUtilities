using System.IO;
using UnityEngine;

namespace NemoUtility
{
    public class MyJsonUtility<T> where T : class, new()
    {
        public static void SaveData(string filePath, T @class)
        {
            string json = JsonUtility.ToJson(@class);
            File.WriteAllText(filePath, json);
        }

        public static T Load(string filePath)
        {
            var result = new T();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(json, result);
            }
            else
            {
                return null;
            }

            return result;
        }
        public static string GetFileText(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return json;
            }
            else
            {
                return "";
            }
        }

        public static string GetString(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return json;
            }
            else
            {
                return "null";
            }
        }
    }
}