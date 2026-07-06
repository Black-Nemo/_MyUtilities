using System.IO;
using UnityEngine;
#if USE_NEWTONSOFT_JSON
using Newtonsoft.Json;//com.unity.nuget.newtonsoft-json
#endif

namespace NemoUtility
{
    public class MyJsonUtility<T> where T : class, new()
    {
        public static void SaveData(string filePath, T @class)
        {
#if USE_NEWTONSOFT_JSON
            string json = JsonConvert.SerializeObject(@class);
#else
            string json = JsonUtility.ToJson(@class);
#endif
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
#if USE_NEWTONSOFT_JSON
                string defaultJson = JsonConvert.SerializeObject(defaultObj);
#else
                string defaultJson = JsonUtility.ToJson(defaultObj);
#endif
                File.WriteAllText(filePath, defaultJson);
            }

            string json = File.ReadAllText(filePath);
            T result = new T();
#if USE_NEWTONSOFT_JSON
            result = JsonConvert.DeserializeObject<T>(json);
#else
            result = JsonUtility.FromJson<T>(json);
#endif
            return result;
        }

        public static void Save(string filePath, T data)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

#if USE_NEWTONSOFT_JSON
            string json = JsonConvert.SerializeObject(data);
#else
            string json = JsonUtility.ToJson(data);
#endif
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