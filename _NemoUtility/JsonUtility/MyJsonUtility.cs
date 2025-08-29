using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace NemoUtility
{
    public class MyJsonUtility<T> where T : class, new()
    {
        public static void SaveData(string filePath, T @class)
        {
            string json = JsonConvert.SerializeObject(@class);
            File.WriteAllText(filePath, json);
        }

        public static T Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                string dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // Varsayılan boş obje olarak kaydet
                T defaultObj = new T();
                string defaultJson = JsonConvert.SerializeObject(defaultObj);
                File.WriteAllText(filePath, defaultJson);
            }

            string json = File.ReadAllText(filePath);
            T result = new T();
            result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        public static void Save(string filePath, T data)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, json);
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