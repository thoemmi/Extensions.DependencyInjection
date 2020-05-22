using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ThomasFreudenberg.Microsoft.Extensions.DependencyInjection;
using ThomasFreudenberg.Microsoft.Extensions.DependencyInjection.Tests;
using Xunit;

namespace Thoemmi.Microsoft.Extensions.DependencyInjection.Tests
{
    public class ScopedTests
    {
        [Fact]
        public void can_resolve_scoped_class()
        {
            var services = new ServiceCollection();
            services.AddNamedScoped<IPlugin, PluginA>("A");
            services.AddNamedScoped<IPlugin, PluginB>("B");

            using var serviceProvider = services.BuildServiceProvider();
            var pluginA = serviceProvider.GetNamedService<IPlugin>("A");

            pluginA
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PluginA>();
        }

        [Fact]
        public void getting_same_named_service_should_return_same_instance()
        {
            var services = new ServiceCollection();
            services.AddNamedScoped<IPlugin, PluginA>("A");
            services.AddNamedScoped<IPlugin, PluginB>("B");

            using var serviceProvider = services.BuildServiceProvider();
            var pluginA1 = serviceProvider.GetNamedService<IPlugin>("A");
            var pluginA2 = serviceProvider.GetNamedService<IPlugin>("A");

            pluginA1
                .Should()
                .BeSameAs(pluginA2);
        }

        [Fact]
        public void can_scoped_transient_class_in_scope()
        {
            var services = new ServiceCollection();
            services.AddNamedScoped<IPlugin, PluginA>("A");
            services.AddNamedScoped<IPlugin, PluginB>("B");

            using var scope = services.BuildServiceProvider().CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var pluginB = serviceProvider.GetNamedService<IPlugin>("B");

            pluginB
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PluginB>();
        }

        [Fact]
        public void getting_same_named_service_with_different_scopes_should_return_different_instance()
        {
            var services = new ServiceCollection();
            services.AddNamedScoped<IPlugin, PluginA>("A");
            services.AddNamedScoped<IPlugin, PluginB>("B");

            using var serviceProvider = services.BuildServiceProvider();
            using var scope1 = serviceProvider.CreateScope();
            using var scope2 = serviceProvider.CreateScope();
            var pluginA1 = scope1.ServiceProvider.GetNamedService<IPlugin>("A");
            var pluginA2 = scope2.ServiceProvider.GetNamedService<IPlugin>("A");

            pluginA1
                .Should()
                .NotBeSameAs(pluginA2);
        }

        [Fact]
        public void getting_not_registered_class_returns_null()
        {
            var services = new ServiceCollection();

            using var serviceProvider = services.BuildServiceProvider();
            var pluginA = serviceProvider.GetNamedService<IPlugin>("A");

            pluginA.Should().BeNull();
        }
    }
}
