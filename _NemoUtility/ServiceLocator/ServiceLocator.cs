using UnityEngine;

namespace NemoUtility
{
    public static class ServiceLocator<T> where T : class, IService
    {
        private static T _service;

        private static bool IsNull(T service)
        {
            if (service == null) return true;
            if (service is UnityEngine.Object obj) return obj == null;
            return false;
        }

        public static void AddService(T service)
        {
            if (!IsNull(_service))
            {
                Debug.LogWarning($"{typeof(T)} already registered");
                return;
            }
            _service = service;
        }

        public static T GetService()
        {
            if (IsNull(_service))
            {
                Debug.LogError($"{typeof(T)} is not registered in the Service Locator!");
                return null;
            }
            return _service;
        }

        public static bool TryGetService(out T service)
        {
            if (!IsNull(_service))
            {
                service = _service;
                return true;
            }
            service = null;
            return false;
        }

        public static bool IsRegistered()
        {
            return !IsNull(_service);
        }

        public static void RemoveService()
        {
            if (IsNull(_service))
            {
                Debug.LogWarning($"{typeof(T)} already no");
                return;
            }
            _service = null;
        }
    }
}