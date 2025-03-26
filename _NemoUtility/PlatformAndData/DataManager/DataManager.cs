using System;
using UnityEngine;

namespace NemoUtility
{
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
                    return GetInt(id);
                }
                else
                {
                    throw new Exception404("404");
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
                throw new Exception404("404");
            }
        }

        public float GetFloat(string id)
        {
            var data = PlatformManager.Instance.GetData(id);
            if (data is float d)
            {
                return d;
            }
            else
            {
                throw new Exception404("404");
            }
        }

        public bool GetBool(string id)
        {
            var data = PlatformManager.Instance.GetData(id);
            if (data is bool d)
            {
                return d;
            }
            else
            {
                throw new Exception404("404");
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
