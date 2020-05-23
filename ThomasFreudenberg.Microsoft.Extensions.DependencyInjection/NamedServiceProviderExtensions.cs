using System;
using Microsoft.Extensions.DependencyInjection;

namespace ThomasFreudenberg.Microsoft.Extensions.DependencyInjection
{
    public static class NamedServiceProviderExtensions
    {
        public static TService? GetNamedService<TService>(this IServiceProvider serviceProvider, string reportName) where TService: class
        {
            var registry = serviceProvider.GetService<NamedServiceRegistry<TService>>();
            if (registry == null)
            {
                // if no named service has been registered, there won't even
                // be a NamedServiceRegistry
                return null;
            }
            var implementationType = registry.GetImplementationType(reportName);

            return (TService?)serviceProvider.GetService(implementationType);
        }

        public static TService GetRequiredNamedService<TService>(this IServiceProvider serviceProvider, string reportName)
        {
            var registry = serviceProvider.GetService<NamedServiceRegistry<TService>>();
            if (registry == null)
            {
                throw new ArgumentException("No named service has been registered.");
            }

            var implementationType = registry.GetImplementationType(reportName);

            return (TService)serviceProvider.GetRequiredService(implementationType);
        }
    }
}