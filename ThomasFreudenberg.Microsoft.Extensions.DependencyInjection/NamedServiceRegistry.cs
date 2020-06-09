using System;
using System.Collections.Generic;

namespace ThomasFreudenberg.Microsoft.Extensions.DependencyInjection
{
    public class NamedServiceRegistry<TService>
    {
        private readonly Dictionary<string, List<Type>> _registeredImplementations = new Dictionary<string, List<Type>>(StringComparer.OrdinalIgnoreCase);

        public void Register<TImplementation>(string serviceName) where TImplementation : TService
        {
            Register(serviceName, typeof(TImplementation));
        }

        public void Register(string serviceName, Type implementationType)
        {
            if (!typeof(TService).IsAssignableFrom(implementationType))
            {
                throw new ArgumentException($"{implementationType} does not implement {typeof(TService)}", nameof(implementationType));
            }

            if (!_registeredImplementations.TryGetValue(serviceName, out var serviceTypes))
            {
                serviceTypes = new List<Type>();
                _registeredImplementations.Add(serviceName, serviceTypes);
            }

            serviceTypes.Add(implementationType);
        }

        public IReadOnlyList<Type> GetImplementationTypes(string serviceName)
        {
            return _registeredImplementations.TryGetValue(serviceName, out var serviceTypes)
                ? (IReadOnlyList<Type>) serviceTypes 
                : Array.Empty<Type>();
        }
    }
}
