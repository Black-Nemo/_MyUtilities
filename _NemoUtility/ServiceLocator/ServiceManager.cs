using System;
using System.Collections.Generic;
using UnityEngine;


namespace NemoUtility
{
    [DefaultExecutionOrder(-1000)]
    public class ServiceManager : MonoBehaviour
    {
        public List<MonoBehaviour> Services;

        private void Awake()
        {
            AddServices();
        }

        private void AddServices()
        {
            foreach (var service in Services)
            {
                if (service is IService)
                {
                    Type type = service.GetType();
                    Type genericType = typeof(ServiceLocator<>).MakeGenericType(type);

                    var addMethod = genericType.GetMethod("AddService");
                    addMethod.Invoke(null, new object[] { service });
                }
                else
                {
                    Debug.LogWarning($"{service.name} does not implement IService and will not be registered.");
                }
            }
        }

        private void RemoveServices()
        {
            foreach (var service in Services)
            {
                if (service is IService)
                {
                    Type type = service.GetType();
                    Type genericType = typeof(ServiceLocator<>).MakeGenericType(type);

                    var addMethod = genericType.GetMethod("RemoveService");
                    addMethod.Invoke(null, new object[] { });
                }
            }
        }

        private void OnDisable()
        {
            RemoveServices();
        }
    }
}