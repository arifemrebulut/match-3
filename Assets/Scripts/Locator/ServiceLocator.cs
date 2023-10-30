using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agave
{
    public class ServiceLocator : MonoBehaviour
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        public static ServiceLocator Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        public void Register<TService>(TService service, bool safe = true) where TService : IService, new()
        {
            var serviceType = typeof(TService);
            if (IsRegistered<TService>() && safe)
            {
                throw new ServiceLocatorException($"{serviceType.Name} has been already registered.");
            }

            Services[typeof(TService)] = service;
        }

        public TService Get<TService>() where TService : IService, new()
        {
            var serviceType = typeof(TService);
            if (IsRegistered<TService>())
            {
                return (TService)Services[serviceType];
            }

            throw new ServiceLocatorException($"{serviceType.Name} hasn't been registered.");
        }

        private bool IsRegistered(Type t)
        {
            return Services.ContainsKey(t);
        }

        private bool IsRegistered<TService>()
        {
            return IsRegistered(typeof(TService));
        }
    }

    public class ServiceLocatorException : Exception
    {
        public ServiceLocatorException(string message) : base(message)
        {
        }
    }
}