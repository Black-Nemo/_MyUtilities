using UnityEngine;

namespace NemoUtility
{
    public static class ServiceLocator<T> where T : class, IService
    {
        private static T _service;
        public static void AddService(T service)
        {
            if (_service != null)
            {
                Debug.LogWarning($"{typeof(T)} already registered");
                return;
            }
            _service = service;
        }

        public static T GetService()
        {
            if (_service == null)
            {
                Debug.LogError($"{typeof(T)} is not registered in the Service Locator!");
                return null;
            }
            return _service;
        }

        public static bool TryGetService(out T service)
        {
            if (_service != null)
            {
                service = _service;
                return true;
            }
            service = null;
            return false;
        }

        public static bool IsRegistered()
        {
            return (_service != null);
        }

        public static void RemoveService()
        {
            if (_service == null)
            {
                Debug.LogWarning($"{typeof(T)} already no");
                return;
            }
            _service = null;
        }
    }
}