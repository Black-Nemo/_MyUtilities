using System;
using UnityEngine;

namespace NemoUtility
{
    [DefaultExecutionOrder(-1000)]
    public class DataManager : MonoBehaviour
    {
        public Action<string, object> SetDataEvent;

        public static DataManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public bool HasData(string id)
        {
            return PlatformManager.Instance.GetData(id) != null;
        }

        public int GetInt(string id, bool IfNullSave = false)
        {
            var data = PlatformManager.Instance.GetData(id);
            if (data is int d)
            {
                return d;
            }
            else if (data is long d2)
            {
                return (int)d2;
            }
            else
            {
                if (IfNullSave)
                {
                    SetInt(id, 0);
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
        public string GetString(string id)
        {
            var data = PlatformManager.Instance.GetData(id);
            if (data is string d)
            {
                return d;
            }
            else
            {
                return "";
            }
        }

        public float GetFloat(string id)
        {
            var data = PlatformManager.Instance.GetData(id);
            if (data is float f)
            {
                return f;
            }
            else if (data is double d)
            {
                return (float)d;
            }
            else if (data is int i)
            {
                return (float)i;
            }
            else if (data is long l)
            {
                return (float)l;
            }
            else
            {
                return 0f;
            }
        }

        public bool GetBool(string id, bool IfNullSave = false)
        {
            var data = PlatformManager.Instance.GetData(id);
            if (data is bool d)
            {
                return d;
            }
            else if (data is long l)
            {
                return l != 0;
            }
            else if (data is int i)
            {
                return i != 0;
            }
            else if (data is double dbl)
            {
                return dbl != 0;
            }
            else if (data is string s)
            {
                return s.Equals("true", System.StringComparison.OrdinalIgnoreCase) || s == "1";
            }
            else
            {
                if (IfNullSave)
                {
                    SetBool(id, false);
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        //Setter
        public void SetInt(string id, int value)
        {
            SetData(id, value);
        }
        public void SetString(string id, string value)
        {
            SetData(id, value);
        }

        public void SetFloat(string id, float value)
        {
            SetData(id, value);
        }

        public void SetBool(string id, bool value)
        {
            SetData(id, value);
        }

        public void SetData(string id, object value)
        {
            PlatformManager.Instance.SetData(id, value);
            SetDataEvent?.Invoke(id, value);
        }

        //Add
        public void AddInt(string id, int value, bool IfNullSave = false)
        {
            var temp = GetInt(id, IfNullSave);
            SetInt(id, temp + value);
        }
        public void AddFloat(string id, float value, bool IfNullSave = false)
        {
            var temp = GetFloat(id);
            SetFloat(id, temp + value);
        }
    }

}
