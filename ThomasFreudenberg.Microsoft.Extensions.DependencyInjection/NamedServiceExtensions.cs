﻿using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ThomasFreudenberg.Microsoft.Extensions.DependencyInjection
{
    public static class NamedServiceExtensions
    {
        public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, string reportName)
            where TService : class
            where TImplementation : class, TService
        {
            var registry = services.GetServiceRegistry<TService>();
            registry.Register<TImplementation>(reportName);

            services.AddTransient<TImplementation>();

            return services;
        }

        public static IServiceCollection AddNamedScoped<TService, TImplementation>(this IServiceCollection services, string reportName)
            where TService : class
            where TImplementation : class, TService
        {
            var registry = services.GetServiceRegistry<TService>();
            registry.Register<TImplementation>(reportName);

            services.AddScoped<TImplementation>();

            return services;
        }

        private static NamedServiceRegistry<TService> GetServiceRegistry<TService>(this IServiceCollection services)
        {
            NamedServiceRegistry<TService> registry;
            var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(NamedServiceRegistry<TService>));
            if (descriptor != null)
            {
                registry = (NamedServiceRegistry<TService>)descriptor.ImplementationInstance;
            }
            else
            {
                registry = new NamedServiceRegistry<TService>();
                services.AddSingleton(registry);
            }

            return registry;
        }
    }
}