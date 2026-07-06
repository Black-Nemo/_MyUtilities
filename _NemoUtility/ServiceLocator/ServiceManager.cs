using System;
using System.Collections.Generic;
using UnityEngine;


namespace NemoUtility
{
    [DefaultExecutionOrder(-1000)]
    public class ServiceManager : MonoBehaviour
    {
        public List<Service> Services;

        private void Awake()
        {
            AddServices();
        }

        private void AddServices()
        {
            foreach (var service in Services)
            {
                if (service.MonoService is IService)
                {

                    Type type = service.MonoService.GetType();
                    Type genericType = typeof(ServiceLocator<>).MakeGenericType(type);

                    var isRegisteredMethod = genericType.GetMethod("IsRegistered");
                    if ((bool)isRegisteredMethod.Invoke(null, null))
                    {
                        Debug.Log($"{service.MonoService.name} is already registered.");
                        continue;
                    }

                    var addMethod = genericType.GetMethod("AddService");
                    addMethod.Invoke(null, new object[] { service.MonoService });
                }
                else
                {
                    Debug.LogWarning($"{service.MonoService.name} does not implement IService and will not be registered.");
                }
            }
        }

        private void RemoveServices()
        {
            foreach (var service in Services)
            {
                if (service.MonoService is IService)
                {
                    Type type = service.MonoService.GetType();
                    Type genericType = typeof(ServiceLocator<>).MakeGenericType(type);

                    var addMethod = genericType.GetMethod("RemoveService");
                    addMethod.Invoke(null, new object[] { });
                }
            }
        }

        private void OnDisable()
        {
            foreach (var service in Services)
            {
                if (service.DestroyDisable)
                {
                    Type type = service.MonoService.GetType();
                    Type genericType = typeof(ServiceLocator<>).MakeGenericType(type);

                    var addMethod = genericType.GetMethod("RemoveService");
                    addMethod.Invoke(null, new object[] { });
                }
            }
            //RemoveServices();
        }
    }
    [System.Serializable]
    public class Service
    {
        public MonoBehaviour MonoService;
        public bool DestroyDisable;
    }
}