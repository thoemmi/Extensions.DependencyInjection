using System;
using System.Collections.Generic;

namespace ThomasFreudenberg.Microsoft.Extensions.DependencyInjection
{
    public class NamedServiceRegistry<TService>
    {
        private readonly Dictionary<string, Type> _registeredImplementations = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public void Register<TImplementation>(string serviceName) where TImplementation : TService
        {
            _registeredImplementations.Add(serviceName, typeof(TImplementation));
        }

        public void Register(string serviceName, Type implementationType)
        {
            if (!(typeof(TService)).IsAssignableFrom(implementationType))
            {
                throw new ArgumentException($"{implementationType} does not implement {typeof(TService)}", nameof(implementationType));
            }

            _registeredImplementations.Add(serviceName, implementationType);
        }

        public Type GetImplementationType(string serviceName)
        {
            if (!_registeredImplementations.TryGetValue(serviceName, out var serviceType))
            {
                throw new ArgumentException($"No service with name {serviceName} has been registered");
            }

            return serviceType;
        }
    }
}
