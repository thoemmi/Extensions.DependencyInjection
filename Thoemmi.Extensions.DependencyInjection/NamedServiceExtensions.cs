using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Thoemmi.Extensions.DependencyInjection
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

        public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, string reportName, TImplementation instance)
            where TService : class
            where TImplementation : class, TService
        {
            var registry = services.GetServiceRegistry<TService>();
            registry.Register(reportName, typeof(TImplementation));

            services.AddTransient(sp => instance);

            return services;
        }

        public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, string reportName, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            var registry = services.GetServiceRegistry<TService>();
            registry.Register(reportName, typeof(TImplementation));

            services.AddTransient(implementationFactory);

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

        public static IServiceCollection AddNamedScoped<TService, TImplementation>(this IServiceCollection services, string reportName, TImplementation instance)
            where TService : class
            where TImplementation : class, TService
        {
            var registry = services.GetServiceRegistry<TService>();
            registry.Register<TImplementation>(reportName);

            services.AddScoped(sp => instance);

            return services;
        }

        public static IServiceCollection AddNamedScoped<TService, TImplementation>(this IServiceCollection services, string reportName, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
        {
            var registry = services.GetServiceRegistry<TService>();
            registry.Register<TImplementation>(reportName);

            services.AddScoped(implementationFactory);

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