using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ThomasFreudenberg.Microsoft.Extensions.DependencyInjection
{
    public static class NamedServiceProviderExtensions
    {
        public static TService? GetNamedService<TService>(this IServiceProvider serviceProvider, string serviceName) where TService: class
        {
            var registry = serviceProvider.GetService<NamedServiceRegistry<TService>>();
            if (registry == null)
            {
                // if no named service has been registered, there won't even
                // be a NamedServiceRegistry
                return null;
            }

            var implementationType = registry.GetImplementationTypes(serviceName).FirstOrDefault();
            if (implementationType == null)
            {
                return null;
            }

            return (TService?)serviceProvider.GetService(implementationType);
        }

        public static IEnumerable<TService> GetNamedServices<TService>(this IServiceProvider serviceProvider, string serviceName) where TService: class
        {
            var registry = serviceProvider.GetService<NamedServiceRegistry<TService>>();
            if (registry == null)
            {
                // if no named service has been registered, there won't even
                // be a NamedServiceRegistry
                return Enumerable.Empty<TService>();
            }

            var implementationTypes = registry.GetImplementationTypes(serviceName);

            return implementationTypes.Select(type => (TService)serviceProvider.GetService(type));
        }

        public static TService GetRequiredNamedService<TService>(this IServiceProvider serviceProvider, string serviceName)
        {
            var registry = serviceProvider.GetService<NamedServiceRegistry<TService>>();
            if (registry == null)
            {
                throw new ArgumentException($"No service with name {serviceName} has been registered.", nameof(serviceName));
            }

            var implementationType = registry.GetImplementationTypes(serviceName).FirstOrDefault();
            if (implementationType == null)
            {
                throw new ArgumentException($"No service with name {serviceName} has been registered.", nameof(serviceName));
            }

            return (TService)serviceProvider.GetRequiredService(implementationType);
        }

        public static IEnumerable<TService> GetRequiredNamedServices<TService>(this IServiceProvider serviceProvider, string serviceName)
        {
            var registry = serviceProvider.GetService<NamedServiceRegistry<TService>>();
            if (registry == null)
            {
                throw new ArgumentException($"No service with name {serviceName} has been registered.", nameof(serviceName));
            }

            var implementationTypes = registry.GetImplementationTypes(serviceName);
            if (implementationTypes == null || !implementationTypes.Any())
            {
                throw new ArgumentException($"No service with name {serviceName} has been registered.", nameof(serviceName));
            }

            return implementationTypes.Select(type => (TService)serviceProvider.GetService(type));
        }
    }
}