using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agave
{
    public class ServiceLocator : MonoBehaviour
    {
        private static readonly Dictionary<Type, object> Services = new ();

        public static void Register<TService>(TService service, bool safe = true) where TService : IService
        {
            var serviceType = typeof(TService);
            if (IsRegistered<TService>() && safe)
            {
                throw new ServiceLocatorException($"{serviceType.Name} has been already registered.");
            }

            Services[typeof(TService)] = service;
        }

        public static TService Get<TService>() where TService : IService
        {
            var serviceType = typeof(TService);
            if (IsRegistered<TService>())
            {
                return (TService)Services[serviceType];
            }

            throw new ServiceLocatorException($"{serviceType.Name} hasn't been registered.");
        }

        private static bool IsRegistered(Type t)
        {
            return Services.ContainsKey(t);
        }

        private static bool IsRegistered<TService>()
        {
            return IsRegistered(typeof(TService));
        }
    }

    public class ServiceLocatorException : Exception
    {
        public ServiceLocatorException(string message) : base(message) { }
    }
}